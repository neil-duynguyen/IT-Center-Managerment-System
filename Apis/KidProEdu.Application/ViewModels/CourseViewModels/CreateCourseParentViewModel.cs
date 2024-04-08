using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CourseViewModels
{
    public class CreateCourseParentViewModel
    {
        public string CourseCode { get; set; }
        public double Price { get; set; }
        public int DurationTotal { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Syllabus { get; set; }
        public int? Level { get; set; }
        public int? EntryPoint { get; set; }
        public string? Image { get; set; }
        public int CourseType { get; set; }
    }
}
