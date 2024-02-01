using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class LocationTrainingProgram : BaseEntity 
    {
        [ForeignKey("Location")]
        public Guid LocationId { get; set; }
        [ForeignKey("TrainingProgram")]
        public Guid TrainingProgramId { get; set; }
        public StatusOfLocationTrainingProgram? Status {  get; set; }

        public virtual Location Location { get; set; }
        public virtual TrainingProgram TrainingProgram { get; set; }
    }
}
