using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Schedule : BaseEntity
    {
        [ForeignKey("Class")]
        public Guid ClassId { get; set; }
        public string Slot { get; set; }
        public Guid ChildrenId { get; set; }
        public TimeSpan StartSlot { get; set; }
        public TimeSpan EndSlot { get; set; }
        public virtual Class Class { get; set; }
        public IList<ScheduleRoom> ScheduleRooms { get; set; }
    }
}
