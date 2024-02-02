using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public Guid UserId { get; set; }
        public string SkillName { get; set; }
        public double Level { get; set; } 
    }
}
