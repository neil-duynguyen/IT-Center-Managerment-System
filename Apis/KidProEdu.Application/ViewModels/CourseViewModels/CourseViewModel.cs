using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CourseViewModels
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationTotal { get; set; }
        public string? Syllabus { get; set; }
        public string? Level { get; set; }
        public string? EntryPoint { get; set; }
        public string? Prerequisite { get; set; }
        public string? Image { get; set; }
        public string CourseType { get; set; }
        public ICollection<CourseViewModel> Courses { get; set; }
    }
}
