using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.SemesterViewModels
{
    public class SemesterViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public StatusSemester StatusSemester { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       /* public IList<SemesterCourse> SemesterCourses { get; set; }
        public IList<Class> Classes { get; set; }*/
    }
}
