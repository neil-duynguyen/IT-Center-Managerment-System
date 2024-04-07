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
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        private readonly AppDbContext _dbContext;

        public ScheduleRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Schedule>> GetScheduleByClass(Guid classId)
        {
            var schedules = await _dbContext.Schedule
                .Where(x => x.ClassId == classId && x.IsDeleted == false)
                .ToListAsync();

            return schedules;
        }

        public async Task<Schedule> GetScheduleBySlot(Guid slotId)
        {
            var schedule = await _dbContext.Schedule.FirstOrDefaultAsync(x => x.SlotId == slotId && x.IsDeleted == false);
            return schedule;
        }
        
        public async Task<List<Schedule>> GetListScheduleBySlot(Guid slotId)
        {
            var schedules = await _dbContext.Schedule.Where(x => x.SlotId == slotId && x.IsDeleted == false).ToListAsync();
            return schedules;
        }
    }
}
