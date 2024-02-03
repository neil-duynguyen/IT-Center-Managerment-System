using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Room : BaseEntity
    {
        public string? Name { get; set; }
        public StatusOfRoom? Status {  get; set; }
        public ICollection<ScheduleRoom> ScheduleRooms { get; set; }
    }
}
