using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Skill : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string SkillName { get; set; }
        public double Level { get; set; } 
        public virtual User User { get; set; }
    }
}
