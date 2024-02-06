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
        public string? Prerequisites { get; set; }

        public virtual Course Course { get; set; }
        public IList<Question> Questions { get; set; }
        public IList<Document> Documents { get; set; }
    }
}
