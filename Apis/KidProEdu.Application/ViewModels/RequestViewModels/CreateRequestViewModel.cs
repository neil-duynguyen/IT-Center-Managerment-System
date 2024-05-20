using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RequestViewModels
{
    public class CreateRequestViewModel
    {
        public Guid[] UserIds { get; set; } // danh sách người nhận
        public string? RequestDescription { get; set; }
        //public StatusOfRequest? Status { get; set; }
        public string RequestType { get; set; }
        public DateTime? LeaveDate { get; set; }
        public DateTime? TeachingDate { get; set; }
        public Guid? CategoryEquimentId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ReturnDeadline { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? FromClassId { get; set; }
        public Guid? ToClassId { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? ReceiverRefundId { get; set; }
        public string? ChildrenCode { get; set; }
        public string? CourseCode { get; set; }
    }
}
