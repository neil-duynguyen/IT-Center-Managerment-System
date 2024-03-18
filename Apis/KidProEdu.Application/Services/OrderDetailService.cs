using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using KidProEdu.Application.ViewModels.OrderViewModelsV2;
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
            //check xem children có học trùng course ko khi chưa vào DB
            if (updateOrderDetailView.GroupBy(x => new { x.ParentCourseId, x.ChildrenProfildId }).Any(g => g.Count() > 1)) throw new Exception("Có trẻ đăng kí khoá học trùng nhau.");

            foreach (var orderDetail in updateOrderDetailView)
            {
                //check xem children có học trùng course ko khi trong DB đã có rồi
                if (_unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.ChildrenProfileId == orderDetail.ChildrenProfildId && x.CourseId == orderDetail.ParentCourseId).Count() > 0)
                    throw new Exception("Trẻ đã đăng kí khoá học này.");

                var getOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderDetail.OrderId);
                var getPrice = await _unitOfWork.CourseRepository.GetByIdAsync(orderDetail.ParentCourseId);
                var getOrderDetail = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.OrderId == orderDetail.OrderId).FirstOrDefault(x => x.CourseId == orderDetail.ParentCourseId);

                //nếu orderDetaiId đó đã tồn tại
                if (getOrderDetail.ChildrenProfileId is not null)
                {
                    OrderDetail newOrderDetail = new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderDetail.OrderId,
                        CourseId = orderDetail.ParentCourseId,
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
                    getOrderDetail.UnitPrice = getPrice.Price;
                    getOrderDetail.TotalPrice = getPrice.Price;
                    getOrderDetail.PayType = (Domain.Enums.PayType?)orderDetail.PayType;
                    getOrderDetail.InstallmentTerm = orderDetail.InstallmentTerm;
                    _unitOfWork.OrderDetailRepository.Update(getOrderDetail);
                    
                    await _unitOfWork.SaveChangeAsync();
                }

                //lấy số tiền đơn hàng trc đó để cộng dồn lên
                var getPriceChildOrderDetail = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.OrderId == orderDetail.OrderId).Sum(x => x.TotalPrice);

                _unitOfWork.OrderRepository.Update(getOrder);
                await _unitOfWork.SaveChangeAsync();
            }


                // orderId = (Guid)getOrderDetail.OrderId;

                //lấy số tiền đơn hàng trc đó để cộng dồn lên
                /*var getPriceChildOrderDetail = _unitOfWork.OrderDetailRepository.GetAllAsync().Result.Where(x => x.ParentOrderDetail == getOrderDetail.Id).Sum(x => x.UnitPrice);

                totalPrice += getPriceChildOrderDetail;


                var getPrice = await _unitOfWork.CourseRepository.GetByIdAsync((Guid)orderDetail.ParentCourseId);
                //course spec thì vào đây
                if (orderDetail.ListChildCourseId.Count > 0)
                {
                    OrderDetail newParentOrderDetail = new()
                    {
                        Id = Guid.NewGuid(),
                        ChildrenProfileId = orderDetail.ChildrenProfildId,
                        CourseId = orderDetail.ParentCourseId,
                        UnitPrice = getPrice.Price,
                    };
                    await _unitOfWork.OrderDetailRepository.AddAsync(newParentOrderDetail);

                    foreach (var item in orderDetail.ListChildCourseId)
                    {
                        var getPriceChildCourse = await _unitOfWork.CourseRepository.GetByIdAsync(item);
                        OrderDetail newChildOrderDetail = new()
                        {
                            Id = Guid.NewGuid(),
                            ChildrenProfileId = orderDetail.ChildrenProfildId,
                            CourseId = item,
                            UnitPrice = getPriceChildCourse.Price,
                            ParentOrderDetail = newParentOrderDetail.Id
                        };
                        totalPrice += getPrice.Price;
                        await _unitOfWork.OrderDetailRepository.AddAsync(newChildOrderDetail);
                    }
                }
                else {                  
                    OrderDetail newOrderDetail = new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderId,
                        ChildrenProfileId = orderDetail.ChildrenProfildId,
                        CourseId = orderDetail.ParentCourseId,
                        UnitPrice = getPrice.Price,
                    };
                    totalPrice += getPrice.Price;
                    await _unitOfWork.OrderDetailRepository.AddAsync(newOrderDetail);
                }*/





                //update TotalPrice bên Order
                //var getOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                //getOrder.TotalAmount = totalPrice;
                // _unitOfWork.OrderRepository.Update(getOrder);

                return true;
        }
    }
}
