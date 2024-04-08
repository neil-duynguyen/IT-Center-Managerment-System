using KidProEdu.Application.ViewModels.ScheduleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class ClassForScheduleViewModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string ClassCode { get; set; }
        public string StatusOfClass { get; set; }
        public int TotalDuration { get; set; }
        public DateTime TeachingStartDate { get; set; } 
        public DateTime TeachingEndDate { get; set; } 
        public List<ScheduleForAutoViewModel> Schedules { get; set; }
    }
}
