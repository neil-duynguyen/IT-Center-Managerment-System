using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class SkillTag : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        [ForeignKey("Tag")]
        public Guid TagId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
