using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.UserViewModels
{
    public class ChangePasswordViewModel
    {
        public Guid id { get; set; }
        public string? currentPassword { get; set; }
        public string newPasswordHash { get; set; } 
    }
}
