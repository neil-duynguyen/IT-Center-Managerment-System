using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CourseViewModels
{
    public class CreateCourseViewModel
    {
        public Guid ParentCourse { get; set; }
        public string CourseCode { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int DurationTotal { get; set; }
        public string? Image { get; set; }
    }
}
