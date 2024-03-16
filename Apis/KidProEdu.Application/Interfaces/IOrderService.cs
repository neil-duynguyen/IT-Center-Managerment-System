using KidProEdu.Application.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(CreateOrderDetailViewModel orderDetailViewModel);
    }
}
