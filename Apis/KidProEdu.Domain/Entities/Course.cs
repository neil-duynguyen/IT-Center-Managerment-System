using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class Course : BaseEntity
    {
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
        public CourseType CourseType { get; set; }
        public Guid ParentCourse { get; set; }
        public IList<Rating> Ratings { get; set; }
        public IList<SemesterCourse> SemesterCourses { get; set; }
        public IList<Class> Classes { get; set; }
        public IList<Lesson> Lessons { get; set; }
        public IList<Score> Scores { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
        public Certificate Certificate { get; set; }
    }
}
