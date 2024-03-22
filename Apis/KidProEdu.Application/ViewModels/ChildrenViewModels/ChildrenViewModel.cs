﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ChildrenViewModels
{
    public class ChildrenViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? GenderType { get; set; }
        public string BirthDay { get; set; }
        public string? Avatar { get; set; }
        public string? SpecialSkill { get; set; }
        public IList<ClassViewModelInChildren> Classes { get; set; }
    }

    public class ClassViewModelInChildren
    {
        public Guid? ClassId { get; set; }
        public string ClassCode { get; set; }
    }
}
