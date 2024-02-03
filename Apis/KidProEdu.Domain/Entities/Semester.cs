using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Semester : BaseEntity
    {
        public string SemesterName { get; set; }
        public DateTime? StartDate { get; set; }
        public IList<SemesterCourse> SemesterCourses { get; set; }
    }
}
