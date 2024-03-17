using KidProEdu.Application.ViewModels.OrderViewModels;
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
        Task<Order> CreateOrder(CreateOrderDetailViewModel orderDetailViewModel);
        Task<bool> CreateTransaction(Guid orderId);
    }
}
