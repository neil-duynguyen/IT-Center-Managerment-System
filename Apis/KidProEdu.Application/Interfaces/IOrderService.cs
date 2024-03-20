using KidProEdu.Application.PaymentService.Momo.Request;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using KidProEdu.Application.ViewModels.OrderViewModelsV2;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderViewModel>> GetOrderByStaffId();
        Task<OrderViewModel> CreateOrder(CreateOrderDetailViewModel orderDetailViewModel);
        //Task<bool> CreateTransaction(Guid orderId);
        Task<string> CreatePaymentHandler(Guid orderId);
        Task<string> ProcessMomoPaymentReturnHandler(MomoOneTimePaymentResultRequest response);
    }
}
