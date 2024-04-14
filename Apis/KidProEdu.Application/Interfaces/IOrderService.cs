using KidProEdu.Application.PaymentService.Momo.Request;
using KidProEdu.Application.PaymentService.VnPay.Response;
using KidProEdu.Application.Services;
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
        Task<bool> CreatePaymentHandler(Guid orderId);
        Task<BaseResult> ProcessMomoPaymentReturnHandler(MomoOneTimePaymentResultRequest response);
        Task<BaseResult> ProcessVnPaymentReturnHandler(VnpayPayResponse response);
    }
}
