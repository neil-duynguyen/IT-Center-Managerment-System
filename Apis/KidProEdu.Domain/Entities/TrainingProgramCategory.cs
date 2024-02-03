using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class TrainingProgramCategory : BaseEntity
    {
        public string Name { get; set; }
        public string LearningAge { get; set; }
        public string Description { get; set; }
        public IList<TrainingProgram> TrainingPrograms { get; set; }
    }
}
