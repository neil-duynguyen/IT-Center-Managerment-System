using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.TrainingProgramViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface ITrainingProgramRepository : IGenericRepository<TrainingProgram>
    {
        Task<TrainingProgram> GetTrainingProgramByTrainingProgramCode(string trainingProgramCode);
        Task<TrainingProgram> GetTrainingProgramByTrainingProgramName(string trainingProgramName);
        Task<TrainingProgram> GetTrainingProgramByProperty(UpdateTrainingProgramViewModel updateTrainingProgramViewModel, Expression<Func<TrainingProgram, object>> property);

    }
}
