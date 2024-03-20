using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class ConfigJobType : BaseEntity
    {
        public JobType JobType { get; set; }
        public int Slotperweek { get; set; }
    }
}

