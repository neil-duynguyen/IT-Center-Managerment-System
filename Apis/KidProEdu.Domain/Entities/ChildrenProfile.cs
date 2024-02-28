using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class ChildrenProfile : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string? GenderType { get; set; }
        public string BirthDay { get; set; }
        public string? Avatar { get; set; }
        public string? SpecialSkill { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public IList<Score> Scores { get; set; }
        public IList<ChildrenCertificate> ChildrenCertificates { get; set; }
        public IList<Enrollment> Enrollments { get; set; }
        public IList<Attendance> Attendances { get; set; }
    }
}
