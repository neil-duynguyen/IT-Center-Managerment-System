using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<List<Attendance>> GetListAttendanceByScheduleIdAndChilId(Guid scheduleId, Guid childId)
        {
            var attendances = await _dbContext.Attendance.Where(x => x.ChildrenProfileId == childId && x.ScheduleId == scheduleId && !x.IsDeleted).ToListAsync();
            return attendances;
        }

        public async Task<List<Attendance>> GetListAttendanceByScheduleIdAndChilIdAndStatusFuture(Guid scheduleId, Guid childId)
        {
            var attendances = await _dbContext.Attendance.Where(x => x.ChildrenProfileId == childId && x.ScheduleId == scheduleId && x.StatusAttendance == StatusAttendance.Future && !x.IsDeleted).ToListAsync();
            return attendances;
        }

        public async Task<List<Attendance>> GetListAttendanceByChilIdAndStatusFuture(Guid childId)
        {
            var attendances = await _dbContext.Attendance.Where(x => x.ChildrenProfileId == childId && x.StatusAttendance == StatusAttendance.Future && !x.IsDeleted).ToListAsync();
            return attendances;
        }

        public async Task<Attendance> GetListAttendanceByClassIdAndChilIdAndOutOfStatusFuture(Guid classId, Guid childId)
        {
            var attendance = _dbContext.Attendance.Include(x => x.Schedule).Where(x => x.ChildrenProfileId == childId && x.Schedule.ClassId == classId && x.StatusAttendance != StatusAttendance.Future && !x.IsDeleted).OrderByDescending(x => x.Date).FirstOrDefault();
            return attendance;
        }

        public async Task<List<Attendance>> GetListAttendanceByCourseIdAndChildId(Guid courseId, Guid childId)
        {
            var attendanceList = await _dbContext.Attendance
                .Include(x => x.Schedule)
                    .ThenInclude(x => x.Slot)
                .Include(x => x.Schedule.Class)
                    .ThenInclude(x => x.Course)
                .Include(x => x.Schedule.Class)
                    .ThenInclude(x => x.TeachingClassHistories)
                        .ThenInclude(x => x.UserAccount)
                .Include(x => x.Schedule)
                    .ThenInclude(x => x.ScheduleRooms)
                        .ThenInclude(x => x.Room)
                .Where(x => x.ChildrenProfileId == childId && x.Schedule.Class.CourseId == courseId && !x.IsDeleted)
                .OrderBy(x => x.Date)
                .ToListAsync();

            return attendanceList;
        }

        public async Task<List<Attendance>> GetListAttendanceByClassIdAndDateAndScheduleId(Guid classId, DateTime date, Guid scheduleId)
        {
            var attendanceList = await _dbContext.Attendance
                .Include(x => x.ChildrenProfile)
                .Where(x => x.Schedule.ClassId == classId && x.Date.Date  == date.Date && x.ScheduleId == scheduleId && !x.IsDeleted)
                .ToListAsync();
            return attendanceList;
        }

        public async Task<List<Attendance>> GetAttendancesByChildId(Guid childId, DateTime startDate, DateTime endDate)
        {
            var attendanceList = await _dbContext.Attendance
                .Include(x => x.Schedule)
                    .ThenInclude(x => x.Slot)
                .Include(x => x.Schedule.Class)
                    .ThenInclude(x => x.Course)
                .Include(x => x.Schedule.Class)
                    .ThenInclude(x => x.TeachingClassHistories)
                        .ThenInclude(x => x.UserAccount)
                .Include(x => x.Schedule)
                    .ThenInclude(x => x.ScheduleRooms)
                        .ThenInclude(x => x.Room)
                .Where(x => x.ChildrenProfileId == childId && x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date && !x.IsDeleted)
                .OrderBy(x => x.Date)
                .ToListAsync();

            return attendanceList;
        }

        public async Task<List<Attendance>> GetListAttendancesByChildId(Guid childId)
        {
            var attendanceList = await _dbContext.Attendance
                .Include(x => x.Schedule)
                .Where(x => x.ChildrenProfileId == childId && !x.IsDeleted)
                .ToListAsync();
            return attendanceList;
        }

        public async Task<List<Attendance>> GetListAttendanceByClassIdAndChilIdOutOfStatusFuture(Guid classId, Guid childId)
        {
            var attendances = await _dbContext.Attendance
                .Include(x => x.Schedule)
                .Where(x => x.ChildrenProfileId == childId && x.Schedule.ClassId == classId && x.StatusAttendance != StatusAttendance.Future && !x.IsDeleted).ToListAsync();
            return attendances;
        }
    }
    
}
