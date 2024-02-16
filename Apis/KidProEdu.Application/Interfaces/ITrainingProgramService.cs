using KidProEdu.Application.ViewModels.TrainingProgramViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<List<TrainingProgram>> GetTrainingPrograms();
        Task<bool> CreateTrainingProgram(CreateTrainingProgramViewModel createTrainingProgramViewModel);
        Task<bool> UpdateTrainingProgram(UpdateTrainingProgramViewModel updateTrainingProgramViewModel, params Expression<Func<TrainingProgram, object>>[] uniqueProperties);
        Task<TrainingProgram> GetTrainingProgramById(Guid id);
        Task<bool> DeleteTrainingProgram(Guid id);
    }
}
