using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //check xem children có học trùng course ko
            if (updateOrderDetailView.GroupBy(x => new { x.ParentCourseId, x.ChildrenProfildId }).Any(g => g.Count() > 1)) throw new Exception("Có trẻ đăng kí khoá học trùng nhau.");

            foreach (var orderDetail in updateOrderDetailView)
            {
                var getOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(orderDetail.OrderDetailId);
                getOrderDetail.PayType = (Domain.Enums.PayType?)orderDetail.PayType;
                getOrderDetail.InstallmentTerm = orderDetail.InstallmentTerm;

                _unitOfWork.OrderDetailRepository.Update(getOrderDetail);

                foreach (var item in orderDetail.ListChildCourseId)
                {
                    OrderDetail newOrderDetail = new()
                    {
                        Id = Guid.NewGuid(),
                        ChildrenProfileId = orderDetail.ChildrenProfildId,
                        CourseId = item,
                        UnitPrice = _unitOfWork.CourseRepository.GetByIdAsync(item).Result.Price,
                        ParentOrderDetail = orderDetail.OrderDetailId
                    };
                }
            }



            return true;
        }
    }
}
