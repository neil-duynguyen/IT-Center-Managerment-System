using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.OrderViewModels
{
    public class CreateOrderDetailViewModel
    {
        public List<OrderDetailViewModel> orderDetailViewModels { get; set; }
    }
    public class OrderDetailViewModel
    {
        public Guid CourseId { get; set; }
        public int Quantity { get; set; }
        public int InstallmentTerm { get; set; }
        public PayType PayType { get; set; }
        public Guid ChildrenProfileId { get; set; }
    }
}
