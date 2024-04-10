using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class ConfigPointMultiplier : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }
        public TestType TestType { get; set; }  
        public double Multiplier { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
