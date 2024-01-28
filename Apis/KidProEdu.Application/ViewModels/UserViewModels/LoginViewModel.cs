﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.LoginViewModel
{
    public class LoginViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public string Token { get; set; }
        public string Role { get; set; }
    }
}
