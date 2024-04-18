using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class ChildrenAnswer : BaseEntity
    {
        [ForeignKey("ChildrenProfile")]
        public Guid ChildrenProfileId { get; set; }
        [ForeignKey("Exam")]
        public Guid ExamId { get; set; }
        [ForeignKey("Question")]
        public Guid? QuestionId { get; set; }
        public string? Answer { get; set; }
        public double ScorePerQuestion { get; set; }
        public virtual Question? Question { get; set; }
        public virtual ChildrenProfile ChildrenProfile { get; set; }
        public virtual Exam Exam { get; set; }
    }
}
