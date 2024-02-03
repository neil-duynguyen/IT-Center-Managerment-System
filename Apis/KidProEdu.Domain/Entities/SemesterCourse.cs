using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class SemesterCourse : BaseEntity
    {
        [ForeignKey("Course")]
        public Guid? CourseId { get; set; }
        [ForeignKey("Semester")]
        public Guid? SemesterId { get; set; }
        public virtual Course? Course { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
