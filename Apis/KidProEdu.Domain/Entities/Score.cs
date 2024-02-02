using System;
using System.Collections.Generic;
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
        public Guid ChildrenId { get; set; }
    }
}
