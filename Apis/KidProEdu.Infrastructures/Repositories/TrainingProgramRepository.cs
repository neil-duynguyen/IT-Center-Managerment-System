using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.ViewModels.TrainingProgramViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{
    public class TrainingProgramRepository : GenericRepository<TrainingProgram>, ITrainingProgramRepository
    {
        private readonly AppDbContext _dbContext;

        public TrainingProgramRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<TrainingProgram> GetTrainingProgramByTrainingProgramCode(string trainingProgramCode)
        {
            var trainingPrograms = await _dbContext.TrainingPrograms
                .Where(x => x.TrainingProgramCode.ToLower() == trainingProgramCode.ToLower() && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return trainingPrograms;
        }
        
        public async Task<TrainingProgram> GetTrainingProgramByTrainingProgramName(string trainingProgramName)
        {
            var trainingPrograms = await _dbContext.TrainingPrograms
                .Where(x => x.TrainingProgramName.ToLower() == trainingProgramName.ToLower() && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return trainingPrograms;
        }

        public async Task<TrainingProgram> GetTrainingProgramByProperty(UpdateTrainingProgramViewModel updateTrainingProgramViewModel, Expression<Func<TrainingProgram, object>> property)
        {
            var body = property.Body;
            MemberExpression memberExpression;

            // Nếu biểu thức lambda là một UnaryExpression, ta cần lấy Operand
            if (body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            // Nếu biểu thức lambda là một MemberExpression
            else if (body is MemberExpression)
            {
                memberExpression = (MemberExpression)body;
            }
            else
            {
                throw new ArgumentException("Invalid expression type");
            }

            var propertyName = memberExpression.Member.Name;

            switch (propertyName)
            {
                case nameof(TrainingProgram.TrainingProgramName):
                    return await GetTrainingProgramByTrainingProgramName(updateTrainingProgramViewModel.TrainingProgramName);
                case nameof(TrainingProgram.TrainingProgramCode):
                    return await GetTrainingProgramByTrainingProgramCode(updateTrainingProgramViewModel.TrainingProgramCode);
                // Thêm các trường hợp xử lý khác nếu cần
                default:
                    throw new ArgumentException($"Property {propertyName} is not supported.");
            }
        }
    }
}
