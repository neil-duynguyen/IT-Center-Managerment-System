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
    public class TeachingClassHistoryRepository : GenericRepository<TeachingClassHistory>, ITeachingClassHistoryRepository
    {
        private readonly AppDbContext _dbContext;
        public TeachingClassHistoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<TeachingClassHistory>> GetClassByTeacherId(Guid id)
        {
            var teachingHistorys = await _dbContext.TeachingClassHistory.Where(x => x.UserAccountId == id
            && x.IsDeleted == false && x.TeachingStatus.Equals(Domain.Enums.TeachingStatus.Teaching)).ToListAsync();

            return teachingHistorys;
        }
    }
}
