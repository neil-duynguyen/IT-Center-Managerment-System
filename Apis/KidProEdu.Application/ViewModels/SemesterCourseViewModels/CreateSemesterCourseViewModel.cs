using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.SemesterCourseViewModels
{
    public class CreateSemesterCourseViewModel
    {
        public Guid SemesterId { get; set; }
        public IList<Guid> CourseId { get; set; }
    }
}
