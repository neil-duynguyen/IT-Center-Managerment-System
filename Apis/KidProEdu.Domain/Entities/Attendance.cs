using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class Attendance : BaseEntity
    {
        [ForeignKey("Schedule")]
        public Guid ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public Guid ChildrenId { get; set; }
        public StatusAttendance StatusAttendance { get; set; }
        public string? Note { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
