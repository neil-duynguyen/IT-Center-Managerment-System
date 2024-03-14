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
        Task<bool> CreateSchedule(CreateScheduleViewModel createScheduleViewModel);
        Task<bool> UpdateSchedule(UpdateScheduleViewModel updateScheduleViewModel);
        Task<ScheduleViewModel> GetScheduleById(Guid scheduleId);
        Task<bool> DeleteSchedule(Guid scheduleId);
    }
}
