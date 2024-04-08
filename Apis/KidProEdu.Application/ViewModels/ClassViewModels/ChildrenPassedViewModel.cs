using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class ChildrenPassedViewModel
    {
        /*public Guid ChildrenId { get; set; }
         public Guid CourseId { get; set; }*/
        public ChildrenProfileViewModel ChildrenProfile { get; set; }
        public CourseViewModel Course { get; set; }
        public ClassViewModel Class { get; set; }
    }
}
