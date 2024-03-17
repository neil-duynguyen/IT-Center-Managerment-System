using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly AppDbContext _dbContext;
        public AttendanceRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }



        public async Task<List<Attendance>> GetAttendanceByScheduleId(Guid id)
        {
            var results = await _dbContext.Attendance
                .Where(x => x.ScheduleId == id && x.IsDeleted == false).Include(x => x.ChildrenProfile)
                .ToListAsync();
            return results;
        }

        public override async Task<List<Attendance>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.ChildrenProfile).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Attendance> GetAttendanceByScheduleIdAndChilId(Guid scheduleId, Guid childId)
        {
            var result = await _dbContext.Attendance.FirstOrDefaultAsync(x => x.ScheduleId == scheduleId && x.ChildrenProfileId == childId && x.IsDeleted == false);
            return result;
        }
    }
}
