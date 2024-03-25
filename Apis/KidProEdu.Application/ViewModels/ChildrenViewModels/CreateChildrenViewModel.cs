﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ChildrenViewModels
{
    public class CreateChildrenViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string? GenderType { get; set; }
        public DateTime BirthDay { get; set; }
        public string Avatar { get; set; }
        public string SpecialSkill { get; set; }
    }
}
