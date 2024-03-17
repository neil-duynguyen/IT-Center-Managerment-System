using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using KidProEdu.Application.ViewModels.OrderViewModelsV2;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
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


    }
}
