using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.TrainingProgramCategoryViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface ITrainingProgramCategoryService
    {
        Task<List<TrainingProgramCategory>> GetTrainingProgramCategories();
        Task<bool> CreateTrainingProgramCategory(CreateTrainingProgramCategoryViewModel createTrainingProgramCategoryViewModel);
        Task<bool> UpdateTrainingProgramCategory(UpdateTrainingProgramCategoryViewModel updateTrainingProgramCategoryViewModel);
        Task<TrainingProgramCategory> GetTrainingProgramCategoryById(Guid id);
        Task<bool> DeleteTrainingProgramCategory(Guid id);
    }
}
