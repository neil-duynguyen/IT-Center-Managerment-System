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
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }
        public string? RequestDescription { get; set; }
        public StatusOfRequest? Status { get; set; }
        public string? RequestType { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string? EquimentType { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? ReceiverRefundId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
