namespace KidProEdu.Application.ViewModels.AdviseRequestViewModels
{
    public class AdviseRequestViewModel
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public bool? IsTested { get; set; }
        public string StatusAdviseRequest { get; set; }
    }
}
