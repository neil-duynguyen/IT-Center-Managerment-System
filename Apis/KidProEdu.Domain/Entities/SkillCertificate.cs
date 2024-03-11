using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class SkillCertificate : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        [ForeignKey("Skill")]
        public Guid SkillId { get; set; }
        public string Url { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
