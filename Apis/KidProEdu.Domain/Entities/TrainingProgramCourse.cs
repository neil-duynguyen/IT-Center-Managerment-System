using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class TrainingProgramCourse : BaseEntity
    {
        public string Name { get; set; }
        public Guid TrainingProgramId { get; set; }
        public Guid CourseId { get; set; }
        public virtual ICollection<TrainingProgram> TrainingProgram { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
