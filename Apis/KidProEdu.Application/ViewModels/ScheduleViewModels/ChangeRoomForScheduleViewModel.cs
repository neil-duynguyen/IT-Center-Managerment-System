using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ScheduleViewModels
{
    public class ChangeRoomForScheduleViewModel
    {
        public Guid ScheduleId { get; set; }
        public Guid RoomId { get; set; }
        public ScheduleRoomStatus Status { get; set; }
    }
}
