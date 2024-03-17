using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.AttendanceViewModels
{
    public class UpdateAttendanceViewModel
    {
        public Guid Id { get; set; }
        public StatusAttendance StatusAttendance { get; set; }
        public string? Note { get; set; }
    }
}
