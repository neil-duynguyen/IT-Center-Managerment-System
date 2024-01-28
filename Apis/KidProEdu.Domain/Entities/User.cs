using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class User : BaseEntity
    {
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public string UserName { get; set; }

        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        
        public string GenderType { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Avata { get;set; }
        public string? OPT { get; set; }
        public virtual Role Role { get; set; }

    }
}
