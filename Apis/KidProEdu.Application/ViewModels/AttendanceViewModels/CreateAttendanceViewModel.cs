using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.AttendanceViewModels
{
    public class CreateAttendanceViewModel
    {
        public Guid ScheduleId { get; set; }
        public DateTime? Date { get; set; }
        public Guid ChildrenProfileId { get; set; }
        public StatusAttendance StatusAttendance { get; set; }
        public string? Note { get; set; }
    }
}
