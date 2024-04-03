using KidProEdu.Application.ViewModels.TeachingClassHistoryViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ScheduleViewModels
{
    public class ScheduleRoomAndTeachingClassHistoryViewModel
    {
        public List<TeachingClassHistory> TeachingClassHistories { get; set; }
        public List<ScheduleRoom> ScheduleRooms { get; set; }
    }
}
