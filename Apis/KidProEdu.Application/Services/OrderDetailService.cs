using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using KidProEdu.Application.ViewModels.OrderViewModelsV2;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KidProEdu.Application.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<List<OrderDetailViewModel>> GetOrderDetailByOrderId(Guid orderId)
        {
            var result = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.OrderId == orderId);
            return _mapper.Map<List<OrderDetailViewModel>>(result);
        }

        public async Task<bool> UpdateOrderDetail(List<UpdateOrderDetailViewModel> updateOrderDetailView)
        {
            var EWalletMethod = string.Empty;

            //check xem children có học trùng course ko khi chưa vào DB
            if (updateOrderDetailView.GroupBy(x => new { x.CourseId, x.ChildrenProfildId }).Any(g => g.Count() > 1)) throw new Exception("Có trẻ đăng kí khoá học trùng nhau.");

            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(5)
            };

            Guid orderId = Guid.Empty;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var orderDetail in updateOrderDetailView)
                    {
                        EWalletMethod = orderDetail.EWalletMethod;
                        orderId = orderDetail.OrderId;
                        //check xem children có học trùng course ko khi trong DB đã có rồi
                        if (_unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.ChildrenProfileId == orderDetail.ChildrenProfildId && x.CourseId == orderDetail.CourseId).Count() > 0)
                            throw new Exception("Trẻ đã đăng kí khoá học này.");

                        var getPrice = await _unitOfWork.CourseRepository.GetByIdAsync(orderDetail.CourseId);
                        var getOrderDetail = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.OrderId == orderDetail.OrderId).FirstOrDefault(x => x.CourseId == orderDetail.CourseId);

                        //nếu orderDetaiId đó đã tồn tại
                        if (getOrderDetail.ChildrenProfileId is not null)
                        {
                            OrderDetail newOrderDetail = new()
                            {
                                Id = Guid.NewGuid(),
                                OrderId = orderDetail.OrderId,
                                CourseId = orderDetail.CourseId,
                                Quantity = 1,
                                UnitPrice = getPrice.Price,
                                TotalPrice = getPrice.Price,
                                ChildrenProfileId = orderDetail.ChildrenProfildId,
                                PayType = (Domain.Enums.PayType?)orderDetail.PayType,
                                InstallmentTerm = orderDetail.InstallmentTerm
                            };
                            await _unitOfWork.OrderDetailRepository.AddAsync(newOrderDetail);
                            await _unitOfWork.SaveChangeAsync();
                        }
                        else
                        {
                            //update OrderDetail
                            getOrderDetail.ChildrenProfileId = orderDetail.ChildrenProfildId;
                            getOrderDetail.Quantity = 1;
                            getOrderDetail.UnitPrice = getPrice.Price;
                            getOrderDetail.TotalPrice = getPrice.Price;
                            getOrderDetail.PayType = (Domain.Enums.PayType?)orderDetail.PayType;
                            getOrderDetail.InstallmentTerm = orderDetail.InstallmentTerm;
                            _unitOfWork.OrderDetailRepository.Update(getOrderDetail);
                            await _unitOfWork.SaveChangeAsync();
                        }
                    }

                    //Update TotalAmount bên Order
                    var updateTotalOrder = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.OrderId == orderId).Sum(x => x.TotalPrice);
                    var getOrderById = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

                    if (getOrderById == null) throw new Exception("Đã xảy ra lỗi không thế cập nhật đơn hàng.");

                    getOrderById.TotalAmount = (double)updateTotalOrder;
                    getOrderById.EWalletMethod = EWalletMethod;
                    _unitOfWork.OrderRepository.Update(getOrderById);
                    await _unitOfWork.SaveChangeAsync();

                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi, transaction scope sẽ tự động rollback
                    Console.WriteLine("Transaction scope rolled back. Error: " + ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<ReturnPaymentInformationView> GetOrderDetailByOrderIdAfterUpdate(Guid orderId)
        {
            List<PaymentInformationView> paymentInformationViews = new List<PaymentInformationView>();
            //tính tổng tiền cần thanh toán
            var getOrderDetail = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.OrderId == orderId).ToList();
            foreach (var item in getOrderDetail)
            {
                if (item.InstallmentTerm > 0)
                {
                    paymentInformationViews.Add(new PaymentInformationView() { CourseCode = item.Course.CourseCode, AmountPerMonth = Math.Ceiling((decimal)(item.TotalPrice / item.InstallmentTerm)), Month = item.InstallmentTerm + " tháng"});
                }
                if (item.InstallmentTerm == 0 && item.PayType.Value == PayType.Banking)
                {
                    paymentInformationViews.Add(new PaymentInformationView() { CourseCode = item.Course.CourseCode, AmountPerMonth = (decimal)item.TotalPrice, Month = "0 tháng" });
                }
                if (item.PayType.Value == PayType.Cash)
                {
                    paymentInformationViews.Add(new PaymentInformationView() { CourseCode = item.Course.CourseCode, AmountPerMonth = (decimal)item.TotalPrice, Month = "0 tháng" });
                }
            }

            List<ReturnOrderDetailViewModel> returnOrderDetailViews = new List<ReturnOrderDetailViewModel>();
            foreach (var item in getOrderDetail)
            {
                returnOrderDetailViews.Add(new ReturnOrderDetailViewModel() { CourseCode = item.Course.CourseCode, Quantity = item.Quantity, UnitPrice = item.UnitPrice, InstallmentTerm = item.InstallmentTerm, PayType = item.PayType.ToString(), ChildrenName = item.ChildrenProfile.FullName });
            }
            var getOrderById = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

            ReturnPaymentInformationView returnPaymentInformationView = new ReturnPaymentInformationView()
            {
                OrderId = orderId,
                returnOrderDetailViews = returnOrderDetailViews,
                paymentInformation = paymentInformationViews,
                CreationDate = getOrderById.CreationDate,
                EWalletMethod = getOrderById.EWalletMethod,
                Total = paymentInformationViews.Sum(x => x.AmountPerMonth)
            };

            return returnPaymentInformationView;
        }
    }
}
