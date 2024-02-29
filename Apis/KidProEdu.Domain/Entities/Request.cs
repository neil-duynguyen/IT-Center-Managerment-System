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
        public string? RequestDescription { get; set; }
        public StatusOfRequest? Status { get; set; }
        public string? RequestType { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string? EquimentType { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? ScheduleId { get; set; }
        public DateTime? TeachingDay { get; set; }
        public Guid? ReceiverRefundId { get; set; }
        public IList<RequestUserAccount> RequestUserAccounts { get; set; }
    }
}
