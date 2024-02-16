using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TrainingProgramViewModels
{
    public class CreateTrainingProgramViewModel
    {
        public Guid TrainingProgramCategoryId { get; set; }
        public string TrainingProgramCode { get; set; }
        public string TrainingProgramName { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
    }
}
