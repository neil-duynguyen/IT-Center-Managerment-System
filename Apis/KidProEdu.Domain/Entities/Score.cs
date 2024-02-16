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
        public double FinalExam { get; set; }
        public Guid CourseId { get; set; }
        public Guid SemesterId { get; set; }
        [ForeignKey("ChildrenProfile")]
        public Guid ChildrenProfileId { get; set; }
        public virtual ChildrenProfile Children { get; set; }
        public virtual Course Course { get; set;}
    }
}
