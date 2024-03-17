using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.OrderDetailViewModels
{
    public class UpdateOrderDetailViewModel
    {
        public Guid OrderDetailId { get; set; }
        public Guid ParentCourseId { get; set; }
        public List<Guid> ListChildCourseId { get; set; }
        public int PayType { get; set; }
        public int? InstallmentTerm { get; set; }
        public Guid ChildrenProfildId { get; set; }
    }
}
