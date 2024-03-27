using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Application.ViewModels.SlotViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ScheduleViewModels
{
    public class ScheduleForAutoViewModel
    {
        public Guid Id { get; set; }
        public string DayInWeek { get; set; }
        public DateTime StartDate { get; set; }
        public SlotForScheduleViewModel SlotForSchedule { get; set; }
        public RoomForScheduleViewModel RoomForSchedule { get; set; }
       
    }
}
