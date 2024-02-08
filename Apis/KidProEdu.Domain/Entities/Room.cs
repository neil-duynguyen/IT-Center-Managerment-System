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
        public IList<ScheduleRoom> ScheduleRooms { get; set; }
        public IList<Equipment> Equipments { get; set; }
    }
}
