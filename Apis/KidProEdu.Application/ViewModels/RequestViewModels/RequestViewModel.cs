using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RequestViewModels
{
    public class RequestViewModel
    {
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }
        public string? RequestDescription { get; set; }
        public string Status { get; set; }
        public string RequestType { get; set; }
        public DateTime? LeaveDate { get; set; }
        public DateTime? TeachingDate { get; set; }
        public string? EquimentType { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? ReceiverRefundId { get; set; }
    }
}
