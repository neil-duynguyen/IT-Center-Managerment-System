using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class ClassViewModel
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StatusOfClass { get; set; }
        public int MaxNumber { get; set; }
        public int ExpectedNumber { get; set; }
        public int ActualNumber { get; set; }
        public List<ScheduleClassViewModel> scheduleClassViews { get; set; }
    }

    public class ScheduleClassViewModel
    { 
        public string Slot { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? DayInWeek { get; set; }
        public string? RoomName { get; set; }
        public Guid? TeacherId { get; set; }
        public string? TeacherName { get; set; }
    }
}
