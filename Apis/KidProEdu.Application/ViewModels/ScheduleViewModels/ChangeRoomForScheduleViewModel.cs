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
        public Guid NewRoomId { get; set; }
        public DateTime StartDate { get; set; } // ngày nào bắt đầu áp dụng
        public DateTime? EndDate { get; set; } // áp dụng tới ngày nào trong trường hợp chuyển tạm
        public ScheduleRoomStatus Status { get; set; } // trạng thái chuyển luôn hay chuyển tạm
    }
}
