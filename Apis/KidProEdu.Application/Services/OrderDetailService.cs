using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
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


            /*foreach (var item in updateOrderDetailView)
            {
                var getOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(item.OrderDetailId);
                getOrderDetail.ChildCourse = item.ListChildCourseId;
                getOrderDetail.PayType = (Domain.Enums.PayType?)item.PayType;
                getOrderDetail.InstallmentTerm = item.InstallmentTerm;
                getOrderDetail.ChildrenProfileId = item.ChildrenProfildId;
            }*/
            return true;
        }
    }
}
