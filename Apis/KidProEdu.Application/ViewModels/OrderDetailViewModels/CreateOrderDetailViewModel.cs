using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.OrderDetailViewModels
{
    public class CreateOrderDetailViewModel
    {
        public Guid ParentId { get; set; }
        public ICollection<Guid> ListCourseId { get; set; }
    }
}
