using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KidProEdu.Application.Services.OrderDetailService;

namespace KidProEdu.Application.Interfaces
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetailViewModel>> GetOrderDetailByOrderId(Guid orderId);
        Task<ReturnPaymentInformationView> GetOrderDetailByOrderIdAfterUpdate(Guid orderId);
        Task<bool> UpdateOrderDetail(List<UpdateOrderDetailViewModel> updateOrderDetailView);
    }
}
