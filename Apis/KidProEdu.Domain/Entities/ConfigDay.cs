using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class ConfigDay : BaseEntity
    {
        public ShiftType Normal { get; set; }
        public ShiftType Saturday { get; set; }
        public ShiftType Sunday { get; set; }
        public ShiftType Holiday { get; set; }
    }
}
