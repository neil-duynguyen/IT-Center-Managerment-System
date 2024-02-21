using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class SubCourse : BaseEntity
    {
        [ForeignKey("Course")]
        public Guid CourseId { get;set; }
        public string CourseCode { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationTotal { get; set; }
        public string Syllabus { get; set; }
        public string Level { get; set; }
        public string? EntryPoint { get; set; }
        public string? Prerequisite { get; set; }
        public string? Image { get; set; }
        public CourseType courseType { get; set; }
        public virtual Course Course { get; set; }
    }
}
