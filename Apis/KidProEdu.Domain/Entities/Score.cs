using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Score : BaseEntity
    {
        public Double FinalExam { get; set; }
        public Guid CourseId { get; set; }
        public Guid SemesterId { get; set; }
        [ForeignKey("Children")]
        public Guid ChildrenId { get; set; }
        public virtual Children Children { get; set; }
    }
}
