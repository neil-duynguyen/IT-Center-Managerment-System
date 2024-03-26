using KidProEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace KidProEdu.Domain.Entities
{

    public class AdviseRequest : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid? UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Guid? LocationId { get; set; }
        public DateTime TestDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsTested { get; set; }
        public StatusAdviseRequest StatusAdviseRequest { get; set; }
        public Guid? SlotId { get; set; }
        public virtual UserAccount? UserAccount { get; set; }
        public virtual Slot? Slot { get; set; }
        public virtual Location? Location { get; set; }
    }
}
