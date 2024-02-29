using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class UserAccount : BaseEntity
    {
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string? GenderType { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Avatar { get;set; }
        public string? OTP { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankName { get; set; }
        public StatusUser Status { get; set; } = StatusUser.Enable;
        public virtual Role Role { get; set; }
        public IList<AdviseRequest> AdviseRequests { get; set; }
        public IList<Feedback> Feedbacks { get; set; }
        public IList<Class> Classes { get; set; }
        public IList<ChildrenProfile> ChildrenProfile { get; set; }
        public IList<Enrollment> Enrollments { get; set; }
        public IList<NotificationUser> NotificationUsers { get; set; }
        public IList<Blog> Blogs { get; set; }
        public IList<Contract> Contracts { get; set; }
        public IList<Division> Divisions { get; set; }
        public IList<RequestUserAccount> RequestUserAccounts { get; set; }
        public IList<LogEquipment> LogEquipments { get; set; }
        public IList<UserSkill> UserSkills { get; set; }
    }
}
