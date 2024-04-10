using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{
    public class ConfigPointMultiplierRepository : GenericRepository<ConfigPointMultiplier>, IConfigPointMultiplierRepository
    {
        private readonly AppDbContext _dbContext;
        public ConfigPointMultiplierRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
        public async Task<ConfigPointMultiplier> GetConfigPointByTestType(TestType type)
        {
            var config = await _dbContext.ConfigPointMultipliers.FirstOrDefaultAsync(x => x.TestType.Equals(type));

            return config;
        }
    }
}
