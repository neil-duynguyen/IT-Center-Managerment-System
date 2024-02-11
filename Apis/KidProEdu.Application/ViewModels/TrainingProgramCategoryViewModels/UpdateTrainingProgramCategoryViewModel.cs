using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TrainingProgramCategoryViewModels
{
    public class UpdateTrainingProgramCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LearningAge { get; set; }
        public string Description { get; set; }
    }
}
