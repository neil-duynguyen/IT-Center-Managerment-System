using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Request : BaseEntity
    {
        public string? RequestCode { get; set; }
        public string? RequestDescription { get; set; }
        public string? RequestType { get; set; }
        public DateTime? LeaveDate { get; set; }
        public int? Quantity { get; set; }
        public Guid? CategoryEquipmentId { get; set; }
        public DateTime? ReturnDeadline { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? FromClassId { get; set; }
        public Guid? ToClassId { get; set; }
        public Guid? ScheduleId { get; set; }
        public DateTime? TeachingDay { get; set; }
        public Guid? ReceiverRefundId { get; set; }
        public string? CourseCode { get; set; }
        public string? ChildrenCode { get; set; }
        public StatusOfRequest? Status { get; set; }
        public IList<RequestUserAccount> RequestUserAccounts { get; set; }
    }
}
