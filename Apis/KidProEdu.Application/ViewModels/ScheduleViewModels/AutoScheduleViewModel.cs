using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ScheduleViewModels
{
    public class AutoScheduleViewModel
    {
        public int Slot {  get; set; }
        public int CountSchedule { get; set; }
        public int CountRoom { get; set; }
        public List<ClassViewModel> ListClassCount { get; set; }
        public List<ClassViewModel> ListRoomCount { get; set; }
    }
}
