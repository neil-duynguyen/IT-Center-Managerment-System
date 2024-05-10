using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public int? LessonNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public TypeOfPractice? TypeOfPractice { get; set; }
        public int? GroupSize { get; set; }
        public virtual Course Course { get; set; }
        public IList<Question> Questions { get; set; }
        public IList<CategoryEquipment> CategoryEquipments { get; set; }
    }
}
