using KidProEdu.Application.ViewModels.ClassViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CourseViewModels
{
    public class CourseSummariseDetailViewModel
    {
        public int TotalCourse { get; set; }
        public List<ClassSummariseByCourseViewModel> ClassSummariseByCourse { get; set;}
    }
}
