using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Prerequisite : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Guid PrerequisiteCourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Course PrerequisiteCourse { get; set; }
    }
}
