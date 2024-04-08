using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.AttendanceViewModels
{
    public class AttendanceWithChildrenProfileViewModel
    {
        public Guid Id { get; set; }
        public string ChildrenName { get; set; }
        public string Avatar {  get; set; }
        public string StatusAttendance { get; set; }
        public string? Note { get; set; }
    }
}
