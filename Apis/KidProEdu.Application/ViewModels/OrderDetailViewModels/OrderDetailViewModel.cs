using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.OrderDetailViewModels
{
    public class OrderDetailViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public int? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public int? InstallmentTerm { get; set; }
        public string? PayType { get; set; }
        public Guid? ChildrenProfileId { get; set; }
    }
}
