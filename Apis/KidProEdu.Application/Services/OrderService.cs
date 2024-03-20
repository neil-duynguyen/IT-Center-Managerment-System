using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.PaymentService.Dtos;
using KidProEdu.Application.PaymentService.Momo.Config;
using KidProEdu.Application.PaymentService.Momo.Request;
using KidProEdu.Application.PaymentService.Payment.Commands;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using KidProEdu.Application.ViewModels.OrderViewModelsV2;
using KidProEdu.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace KidProEdu.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public OrderService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<List<OrderViewModel>> GetOrderByStaffId()
        {
            var result = _unitOfWork.OrderRepository.GetAllAsync().Result.Where(x => x.UserAccount.Id == _claimsService.GetCurrentUserId).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<OrderViewModel>>(result);
        }

        public async Task<OrderViewModel> CreateOrder(CreateOrderDetailViewModel orderDetailViewModel)
        {
            double total = 0;
            foreach (var item in orderDetailViewModel.ListCourseId)
            {
                var course = await _unitOfWork.CourseRepository.GetByIdAsync(item);
                total += course.Price;
            }

            //new Order
            Order order = new Order()
            {
                Id = Guid.NewGuid(),
                OrderDate = _currentTime.GetCurrentTime(),
                TotalAmount = total,
                PaymentStatus = Domain.Enums.StatusPayment.Unpaid,
                UserId = _claimsService.GetCurrentUserId
            };
            await _unitOfWork.OrderRepository.AddAsync(order);

            //new OrderDetail
            foreach (var item in orderDetailViewModel.ListCourseId)
            {
                var course = await _unitOfWork.CourseRepository.GetByIdAsync(item);
                OrderDetail orderDetail = new OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    CourseId = course.Id,
                    UnitPrice = course.Price,
                };
                await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? _mapper.Map<OrderViewModel>(order) : throw new Exception("Tạo đơn hàng thất bại");

        }

        //function này sẽ chạy sau khi thanh toán thành công
        /*public async Task<bool> CreateTransaction(Guid orderId)
        {
            //xem INP bên vnpay để làm
            //sau khi check trạng thái thanh toán thành công thì cập nhật db và tạo transaction
            var getOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

            if (getOrder is not null && getOrder.PaymentStatus == Domain.Enums.StatusPayment.Paid)
            {
                var getOrderDetail = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.OrderId == orderId && x.PayType == Domain.Enums.PayType.Banking).ToList();

                foreach (var item in getOrderDetail)
                {
                    //trường hợp có trả góp
                    if (item.InstallmentTerm != 0)
                    {
                        //create transaction parent
                        Transaction transactionParent = new Transaction()
                        {
                            Id = Guid.NewGuid(),
                            OrderDetailId = item.Id,
                            BankingAccountNumber = "test",
                            BankingNumber = "test",
                            BankName = "test",
                            CourseName = item.Course.Name,
                            PayDate = _currentTime.GetCurrentTime(), //lấy thời gian thanh toán thành công
                            InstallmentTerm = item.InstallmentTerm,
                            InstallmentPeriod = DateTime.Now, //alow null
                            StatusTransaction = Domain.Enums.StatusTransaction.Installment,

                        };
                        await _unitOfWork.TransactionRepository.AddAsync(transactionParent);

                        //tính số tiên trả góp hàng tháng
                        var installmentPayments = Math.Ceiling(item.TotalPrice / item.InstallmentTerm);
                        //tạo con
                        for (int i = 0; i < transactionParent.InstallmentTerm; i++)
                        {
                            Transaction transactionChild = new Transaction()
                            {
                                Id = Guid.NewGuid(),
                                CourseName = item.Course.Name,
                                TotalAmount = installmentPayments,
                                InstallmentPeriod = i == 0 ? _currentTime.GetCurrentTime() : _currentTime.GetCurrentTime().AddMonths(i),
                                StatusTransaction = i == 0 ? Domain.Enums.StatusTransaction.Successfully : Domain.Enums.StatusTransaction.Pending,
                                ParentsTransaction = transactionParent.Id
                            };
                            await _unitOfWork.TransactionRepository.AddAsync(transactionChild);
                        }

                    }
                    else
                    {
                        //ko trả góp
                        Transaction transactionparent = new Transaction()
                        {
                            BankingAccountNumber = "test",
                            BankingNumber = "test",
                            BankName = "test",
                            CourseName = item.Course.Name,
                            PayDate = _currentTime.GetCurrentTime(),
                            InstallmentTerm = 0,
                            InstallmentPeriod = _currentTime.GetCurrentTime(),
                            StatusTransaction = Domain.Enums.StatusTransaction.Successfully,
                            OrderDetailId = item.Id
                        };
                        await _unitOfWork.TransactionRepository.AddAsync(transactionparent);
                    }
                }
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo transaction thất bại");
            }
            return true;
        }*/


        public async Task<string> CreatePaymentHandler(Guid orderId)
        {
            string result;
            try
            {
                var paymentUrl = string.Empty;
                var getOrderById = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

                if (getOrderById is not null)
                {

                    CreatePayment createPayment = new CreatePayment();
                    createPayment.PaymentDate = DateTime.Now;
                    createPayment.ExpireDate = DateTime.Now.AddMinutes(1);
                    createPayment.PaymentContent = "Thanh toán đơn hàng.";
                    createPayment.RequiredAmount = (decimal?)getOrderById.TotalAmount;

                    switch (createPayment.PaymentDestinationId)
                    {
                        /*case "VNPAY":
                            var vnpayPayRequest = new VnpayPayRequest(vnpayConfig.Version,
                                vnpayConfig.TmnCode, DateTime.Now, currentUserService.IpAddress ?? string.Empty, request.RequiredAmount ?? 0, request.PaymentCurrency ?? string.Empty,
                                "other", request.PaymentContent ?? string.Empty, vnpayConfig.ReturnUrl, outputIdParam!.Value?.ToString() ?? string.Empty);
                            paymentUrl = vnpayPayRequest.GetLink(vnpayConfig.PaymentUrl, vnpayConfig.HashSecret);
                            break;*/
                        case "MOMO":
                            var momoOneTimePayRequest = new MomoOneTimePaymentRequest(
                                _configuration["Momo:PartnerCode"],
                                Guid.NewGuid().ToString(),
                                (long)createPayment.RequiredAmount,
                                getOrderById.Id.ToString(),
                                createPayment.PaymentContent ?? string.Empty,
                                _configuration["Momo:ReturnUrl"],
                                _configuration["Momo:IpnUrl"],
                                "captureWallet",
                                string.Empty);
                            momoOneTimePayRequest.MakeSignature(_configuration["Momo:AccessKey"], _configuration["Momo:SecretKey"]);
                            (bool createMomoLinkResult, string? createMessage) = momoOneTimePayRequest.GetLink(_configuration["Momo:PaymentUrl"]);
                            if (createMomoLinkResult)
                            {
                                paymentUrl = createMessage;
                            }
                            else
                            {
                                result = createMessage;
                            }
                            break;
                        default:
                            break;
                    }

                    return result = paymentUrl;
                }
                throw new Exception("Không tìm thấy đơn hàng.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> ProcessMomoPaymentReturnHandler(MomoOneTimePaymentResultRequest response)
        {
            string returnUrl = string.Empty;
            string resultData = string.Empty;
            try
            {
                //var request = new ProcessMomoPaymentReturn();
                //var resultData = new PaymentReturnDtos();
                //var isValidSignature = request.IsValidSignature(_configuration["Momo:AccessKey"], _configuration["Momo:SecretKey"]);

                if (true)
                {

                    if (response.resultCode == 0)
                    {

                        resultData = "https://kid-pro-edu-v2.netlify.app//payment-success";
                    }
                    else
                    {

                        resultData = "Payment process failed";
                    }
                }
                else
                {

                    resultData = "Invalid signature in response";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return resultData;
        }
    }
    public class CreatePayment
    {
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = "VND";
        public string PaymentRefId { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string? PaymentLanguage { get; set; } = "vn";
        public string? MerchantId { get; set; } = string.Empty;
        public string? PaymentDestinationId { get; set; } = "MOMO";
        public string? Signature { get; set; } = string.Empty;
    }
}
