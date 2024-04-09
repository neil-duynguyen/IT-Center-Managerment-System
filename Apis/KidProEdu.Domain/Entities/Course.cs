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
        public int? Level { get; set; }
        public int? EntryPoint { get; set; } //điểm đầu vào thang điểm 10
        public string? Image { get; set; }
        public CourseType CourseType { get; set; }
        public Guid? ParentCourse { get; set; }
        public IList<Rating> Ratings { get; set; }
        public IList<Class> Classes { get; set; }
        public IList<Lesson> Lessons { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
        public IList<Resource> Resources { get; set; }
        public IList<Certificate> Certificates { get; set; }
        public IList<Exam> Exams { get; set; }
    }
}
