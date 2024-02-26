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
        [ForeignKey("Semester")]
        public Guid SemesterId { get; set; }
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }     
        public virtual Semester Semester { get; set; }
        public virtual Course Course { get; set; }
    }
}
