using KidProEdu.Domain.Enums;

namespace KidProEdu.Application.ViewModels.AdviseRequestViewModels
{
    public class UpdateAdviseRequestViewModel
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? TestDate { get; set; }
        public Guid? SlotId {  get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid LocationId { get; set; }
        public bool? IsTested { get; set; }
        public StatusAdviseRequest StatusAdviseRequest { get; set; }
    }
}
