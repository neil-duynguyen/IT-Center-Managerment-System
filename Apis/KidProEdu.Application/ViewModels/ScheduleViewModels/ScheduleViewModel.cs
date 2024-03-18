using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ScheduleViewModels
{
    public class ScheduleViewModel
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SlotId { get; set; }
        public DateTime? StartDate { get; set; }
        public string? DayInWeek { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        /*public virtual Class Class { get; set; }
        public virtual Slot Slot { get; set; }
        public IList<ScheduleRoom> ScheduleRooms { get; set; }
        public IList<Attendance> Attendances { get; set; }*/
    }
}
