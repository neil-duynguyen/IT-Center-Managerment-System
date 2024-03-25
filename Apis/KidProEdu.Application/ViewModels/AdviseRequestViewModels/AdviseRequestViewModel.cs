using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.ViewModels.AdviseRequestViewModels
{
    public class AdviseRequestViewModel
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public UserViewModel Staff { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Guid LocationId { get; set; }
        public DateTime? TestDate { get; set; }
        public Guid SlotId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsTested { get; set; }
        public string StatusAdviseRequest { get; set; }
    }
}
