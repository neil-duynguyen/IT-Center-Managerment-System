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
        [ForeignKey("Location")]
        public Guid? LocationId { get; set; }
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
        public virtual Location? Location { get; set; }
        public IList<AdviseRequest> AdviseRequests { get; set; }
        public IList<Feedback> Feedbacks { get; set; }
        public IList<TeachingClassHistory> TeachingClassHistories { get; set; }
        public IList<ChildrenProfile> ChildrenProfile { get; set; }
        public IList<Enrollment> Enrollments { get; set; }
        public IList<NotificationUser> NotificationUsers { get; set; }
        public IList<Blog> Blogs { get; set; }
        public IList<Contract> Contracts { get; set; }
        public IList<RequestUserAccount> RequestUserAccounts { get; set; }
        public IList<LogEquipment> LogEquipments { get; set; }
        public IList<SkillCertificate> SkillCertificate { get; set; }
        public IList<DivisionUserAccount> DivisionUserAccounts { get; set; }
        public IList<ConfigPointMultiplier> ConfigPointMultipliers { get; set; }
        public IList<ConfigJobType> ConfigJobTypes { get; set; }
        public IList<ConfigSystem> ConfigSystems { get; set; }
        public IList<ConfigTheme> ConfigThemes { get; set; }
    }
}
