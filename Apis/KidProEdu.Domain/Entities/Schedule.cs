using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public Guid ClassId { get; set; }
        public Guid RoomId { get; set; }
        public string Slot { get; set; }
        public Guid ChildrenId { get; set; }
        public TimeSpan StartSlot { get; set; }
        public TimeSpan EndSlot { get; set; }
    }
}
