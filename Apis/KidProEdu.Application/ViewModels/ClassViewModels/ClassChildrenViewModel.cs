using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class ClassChildrenViewModel
    {
        public Guid Id { get; set; } // id của enrollment 
        public Guid ChildrenProfileId { get; set; }
        public Guid ClassId { get; set; }
        public string Code { get; set; }
        public ChildrenProfile Children { get; set; }
        public string? Avatar { get; set; }
        public string FullName { get; set; }
        public string ChildrenCode { get; set; }
    }
}
