using KidProEdu.Domain.Enums;
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
        public string ChildrenCode { get; set; }
        public string? GenderType { get; set; }
        public DateTime BirthDay { get; set; }
        public string? Avatar { get; set; }
        public string? SpecialSkill { get; set; }
        public StatusChildrenProfile Status { get; set; } = StatusChildrenProfile.Waiting;
        public virtual UserAccount UserAccount { get; set; }
        public IList<Certificate> Certificates { get; set; }
        public IList<Enrollment> Enrollments { get; set; }
        public IList<Attendance> Attendances { get; set; }
        public IList<ChildrenAnswer> ChildrenAnswers { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
    }
}
