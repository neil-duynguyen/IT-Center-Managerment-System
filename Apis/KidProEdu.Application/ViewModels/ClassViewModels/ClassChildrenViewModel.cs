using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class ClassChildrenViewModel
    {
        public Guid ClassId { get; set; }
        public Guid ChildrenId { get; set; }
        public string? Avatar { get; set; }
        public string NameChildren { get;set; }
    }
}
