using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ChildrenViewModels
{
    public class ChildrenProfileViewModel
    {
        public Guid Id { get; set; } // này là id nó childrenId
        public Guid UserId { get; set; } // này là id thằng cha nó
        public string FullName { get; set; }
        public string? GenderType { get; set; }
        public DateTime BirthDay { get; set; }
        public string? Avatar { get; set; }
        public string? SpecialSkill { get; set; }
    }
}
