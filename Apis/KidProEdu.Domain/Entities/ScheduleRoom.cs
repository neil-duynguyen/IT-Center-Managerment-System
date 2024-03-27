﻿using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class ScheduleRoom : BaseEntity
    {
        [ForeignKey("Room")]
        public Guid? RoomId { get; set; }
        [ForeignKey("Schedule")]
        public Guid? ScheduleId { get; set; }
        public DateTime? StartDate { get; set; } //ngày mà lịch đó đổi phòng học tạm thời 1 hôm
        public DateTime? EndDate { get; set; } //ngày mà lịch đó đổi phòng học tạm thời 1 hôm
        public ScheduleRoomStatus? Status { get; set; }
        public virtual Room? Room { get; set; }
        public virtual Schedule? Schedule { get; set; }
    }
}
