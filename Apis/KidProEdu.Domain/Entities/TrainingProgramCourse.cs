using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class TrainingProgramCourse : BaseEntity
    {
        public string Name { get; set; }
        [ForeignKey("TrainingProgram")]
        public Guid TrainingProgramId { get; set; }
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public virtual TrainingProgram TrainingProgram { get; set; }
        public virtual Course Course { get; set;}
    }
}
