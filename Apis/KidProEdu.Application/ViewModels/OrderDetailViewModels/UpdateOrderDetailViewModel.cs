using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.OrderDetailViewModels
{
    public class UpdateOrderDetailViewModel
    {
        public Guid OrderId { get; set; }
        public Guid CourseId { get; set; }
        public int PayType { get; set; }
        public int? InstallmentTerm { get; set; }
        public Guid ChildrenProfildId { get; set; }
        public string? EWalletMethod { get; set; }
    }
}
