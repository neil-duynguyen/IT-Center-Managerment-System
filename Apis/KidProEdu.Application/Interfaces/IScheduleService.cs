using KidProEdu.Application.ViewModels.ScheduleViewModels;
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
    }
}
