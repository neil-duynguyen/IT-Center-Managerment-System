using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string? GenderType { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankName { get; set; }
        public string Status { get; set; }
    }
}
