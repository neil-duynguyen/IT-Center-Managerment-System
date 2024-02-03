using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class User : BaseEntity
    {
        [ForeignKey("Role")]
        
        public Guid? RoleId { get; set; }
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
        public string Status { get; set; } = "Enable";
        public virtual Role? Role { get; set; }
        public IList<AdviseRequest> AdviseRequests { get; set; }
    }
}
