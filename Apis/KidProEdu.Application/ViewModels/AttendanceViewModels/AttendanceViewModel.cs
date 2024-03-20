using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.AttendanceViewModels
{
    public class AttendanceViewModel
    {
        public Guid Id { get; set; }
        public Guid ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public Guid ChildrenProfileId { get; set; }
        public string? StatusAttendance { get; set; }
        public string? Note { get; set; }
        public ChildrenProfileViewModel ChildrenProfile { get; set; }
    }
}
