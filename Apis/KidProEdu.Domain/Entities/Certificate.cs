using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Certificate : BaseEntity
    {
        [ForeignKey("ChildrenProfile")]
        public Guid ChildrenProfileId { get; set; }
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public IList<ChildrenProfile> ChildrenProfiles { get; set; }
        public IList<Course> Courses { get; set; }
    }
}
