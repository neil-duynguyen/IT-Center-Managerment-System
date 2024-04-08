using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class ClassSummariseByCourseViewModel
    {
        public string CourseName { get; set; }
        public int TotalClass { get; set; }
        public double Percent {  get; set; }
        public List<ClassViewModel> ClassList { get; set; }
    }
}
