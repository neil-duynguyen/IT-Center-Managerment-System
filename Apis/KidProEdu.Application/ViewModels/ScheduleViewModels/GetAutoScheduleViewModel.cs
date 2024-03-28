using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ScheduleViewModels
{
    public class GetAutoScheduleViewModel
    {
        //public Guid Id { get; set; } không có trong này
        public Guid TeacherId { get; set; }
        public List<ClassForScheduleViewModel> Classes { get; set; }
        /*public List<Schedule> Schedules { get; set; }
        public List<Room> Rooms { get; set; }*/
    }
}
