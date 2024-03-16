using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderViewModels;
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

        public async Task<bool> CreateOrder(CreateOrderDetailViewModel orderDetailViewModel)
        {
            //new Order
            Order order = new Order()
            {
                Id = Guid.NewGuid(),
                OrderDate = _currentTime.GetCurrentTime(),
                TotalAmount = 0,
                PaymentStatus = Domain.Enums.StatusPayment.Unpaid,
                UserId = _claimsService.GetCurrentUserId
            };
            await _unitOfWork.OrderRepository.AddAsync(order);

            double total = 0;
            foreach (var item in orderDetailViewModel.orderDetailViewModels)
            {
                //new OrderDetail
                var course = await _unitOfWork.CourseRepository.GetByIdAsync(item.CourseId);
                if (course is not null && !course.IsDeleted)
                {
                    if (item.PayType == Domain.Enums.PayType.Banking)
                    {
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            CourseId = item.CourseId,
                            Quantity = item.Quantity,
                            UnitPrice = course.Price,
                            TotalPrice = item.Quantity * course.Price,
                            InstallmentTerm = item.InstallmentTerm,
                            PayType = item.PayType,
                            ChildrenProfileId = item.ChildrenProfileId
                        };
                        await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
                        total += orderDetail.TotalPrice;
                    }
                    else
                    {
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            CourseId = item.CourseId,
                            Quantity = item.Quantity,
                            UnitPrice = course.Price,
                            TotalPrice = item.Quantity * course.Price,
                            InstallmentTerm = item.InstallmentTerm,
                            PayType = item.PayType,
                            ChildrenProfileId = item.ChildrenProfileId
                        };
                        await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
                    }
                }
                else
                {
                    throw new Exception("Tạo đơn hàng thất bại.");
                }
            }
            order.TotalAmount = total;

            var result = await _unitOfWork.SaveChangeAsync();
            if (result == 0)
            {
                throw new DbUpdateException("thất bại");
            }
            return true;
        }


    }
}
