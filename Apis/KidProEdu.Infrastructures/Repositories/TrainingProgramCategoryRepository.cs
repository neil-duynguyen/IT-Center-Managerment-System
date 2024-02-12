using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{

    public class TrainingProgramCategoryRepository : GenericRepository<TrainingProgramCategory>, ITrainingProgramCategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public TrainingProgramCategoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }



        public async Task<List<TrainingProgramCategory>> GetTrainingProgramCategoryByName(string name)
        {
            var trainingProgramCategories = await _dbContext.TrainingProgramCategories
                .Where(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return trainingProgramCategories;
        }
    }
}
