using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Document : BaseEntity
    {
        [ForeignKey("Lesson")]
        public Guid LessonId { get; set; }
        public string? Url { get; set; }
        public Guid ClassId { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
