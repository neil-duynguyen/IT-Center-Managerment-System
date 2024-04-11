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
    public class ScheduleRoomRepository : GenericRepository<ScheduleRoom>, IScheduleRoomRepository
    {
        private readonly AppDbContext _dbContext;
        public ScheduleRoomRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<ScheduleRoom>> GetScheduleRoomByStatus(ScheduleRoomStatus status)
        {
            var scheduleRooms = await _dbContext.ScheduleRoom.Where(x => x.Status.Equals(status)
            && x.IsDeleted == false).ToListAsync();

            return scheduleRooms;
        }

        public async Task<List<ScheduleRoom>> GetScheduleRoomBySchedule(Guid id)
        {
            var scheduleRooms = await _dbContext.ScheduleRoom.Include(x => x.Room).Include(x => x.Schedule).Where(x => x.ScheduleId == id
            && x.IsDeleted == false).ToListAsync();

            return scheduleRooms;
        }

        public async Task<List<ScheduleRoom>> GetScheduleRoomByScheduleAndRoom(Guid scheduleId, Guid roomId)
        {
            var scheduleRooms = await _dbContext.ScheduleRoom.Where(x => x.ScheduleId == scheduleId && x.RoomId == roomId
            && x.IsDeleted == false).ToListAsync();

            return scheduleRooms;
        }

        public override async Task<List<ScheduleRoom>> GetAllAsync()
        {
            return await _dbContext.ScheduleRoom.Include(x => x.Room).Include(x => x.Schedule).Where(x => !x.IsDeleted).ToListAsync();
        }
    }
}
