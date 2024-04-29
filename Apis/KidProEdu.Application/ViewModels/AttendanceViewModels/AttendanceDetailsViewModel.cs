using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.AttendanceViewModels
{
    public class AttendanceDetailsViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public DateTime Date { get; set; }
        public string Slot { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string RoomName { get; set; }
        public string TeacherName { get; set;}
        public string ClassCode { get; set; }
        public string AttendanceStatus { get; set; }
        public string TeacherComment {  get; set; }

    }
}
