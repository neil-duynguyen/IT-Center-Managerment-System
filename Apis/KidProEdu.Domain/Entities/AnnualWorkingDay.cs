using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class AnnualWorkingDay : BaseEntity
    {
        public DateTime Day { get; set; }
        public ShiftType ShiftType { get; set; }
        public TypeDate TypeDate { get; set; }
    }
}
