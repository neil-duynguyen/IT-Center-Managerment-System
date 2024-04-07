using KidProEdu.Application.ViewModels.ScheduleViewModels;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IScheduleService
    {
        Task<List<ScheduleViewModel>> GetSchedules();
        Task<bool> CreateSchedule(CreateScheduleViewModel createScheduleViewModel, Guid classId);
        Task<bool> UpdateSchedule(UpdateScheduleViewModel updateScheduleViewModel);
        Task<ScheduleViewModel> GetScheduleById(Guid scheduleId);
        Task<bool> DeleteSchedule(Guid scheduleId);
        Task<List<AutoScheduleViewModel>> CreateAutomaticalySchedule();
        Task<GetAutoScheduleViewModel> GetAutomaticalySchedule(Guid id);
        Task<ScheduleRoomAndTeachingClassHistoryViewModel> GetScheduleRoomAndTeachingClassHistory();
        Task<bool> ChangeRoomForSchedule(ChangeRoomForScheduleViewModel changeRoomForScheduleViewModel);
        Task<bool> GetEmptyRoomBySlot(Guid scheduleId, Guid slotId, DateTime startDate, DateTime endDate, ScheduleRoomStatus status);
    }
}
