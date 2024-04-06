using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface IScheduleRoomRepository : IGenericRepository<ScheduleRoom>
    {
        Task<List<ScheduleRoom>> GetScheduleRoomByStatus(ScheduleRoomStatus status);
        Task<List<ScheduleRoom>> GetScheduleRoomBySchedule(Guid id);
        Task<List<ScheduleRoom>> GetScheduleRoomByScheduleAndRoom(Guid scheduleId, Guid roomId);
    }
}
