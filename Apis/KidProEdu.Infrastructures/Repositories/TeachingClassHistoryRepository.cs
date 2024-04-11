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
    public class TeachingClassHistoryRepository : GenericRepository<TeachingClassHistory>, ITeachingClassHistoryRepository
    {
        private readonly AppDbContext _dbContext;
        public TeachingClassHistoryRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<TeachingClassHistory>> GetClassByTeacherId(Guid id)
        {
            var teachingHistorys = await _dbContext.TeachingClassHistory
                .Include(x => x.Class).ThenInclude(x => x.Schedules).ThenInclude(x => x.Slot)
                .Include(x => x.Class).ThenInclude(x => x.Course)
                .Where(x => x.UserAccountId == id
                && (x.IsDeleted == false && x.TeachingStatus.Equals(Domain.Enums.TeachingStatus.Teaching)
                || x.TeachingStatus.Equals(Domain.Enums.TeachingStatus.Pending)))
                .ToListAsync();

            return teachingHistorys;
        }

        public async Task<List<TeachingClassHistory>> GetTeachingHistoryByClassId(Guid id)
        {
            var teachingHistorys = await _dbContext.TeachingClassHistory.Include(x => x.UserAccount).Where(x => x.ClassId == id
            && x.IsDeleted == false).ToListAsync();

            return teachingHistorys;
        }

        public async Task<List<TeachingClassHistory>> GetTeachingHistoryByStatus(TeachingStatus status)
        {
            var teachingHistorys = await _dbContext.TeachingClassHistory.Where(x => x.TeachingStatus.Equals(status)
            && x.IsDeleted == false).ToListAsync();

            return teachingHistorys;
        }
        public override async Task<List<TeachingClassHistory>> GetAllAsync()
        {
            return await _dbContext.TeachingClassHistory.Include(x => x.Class).Include(x => x.UserAccount).Where(x => !x.IsDeleted).ToListAsync();
        }

    }
}
