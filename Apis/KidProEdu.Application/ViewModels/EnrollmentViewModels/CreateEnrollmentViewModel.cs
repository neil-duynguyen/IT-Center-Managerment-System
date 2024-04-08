using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EnrollmentViewModels
{
    public class CreateEnrollmentViewModel
    {
        public Guid ClassId { get; set; }
        public List<Guid> ChildrenProfileIds { get; set; }
    }
}
