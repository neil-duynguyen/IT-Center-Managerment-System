using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class TrainingProgram : BaseEntity
    {
        [ForeignKey("TrainingProgramCategory")]
        public Guid TrainingProgramCategoryId { get; set; }
        public string TrainingProgramCode { get; set; }
        public string TrainingProgramName { get; set; }
        public double Price { get; set; }
        public virtual TrainingProgramCategory TrainingProgramCategory { get; set;}
        public ICollection<TrainingProgramCourse> TrainingProgramCourses { get; set; }
    }
}
