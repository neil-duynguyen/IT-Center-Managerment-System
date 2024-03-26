using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.OrderViewModelsV2
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public string? OrderNumber { get; set; }
        public Guid UserId { get; set; } // id parent
        public string FullName { get; set; } //fullname parent
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
