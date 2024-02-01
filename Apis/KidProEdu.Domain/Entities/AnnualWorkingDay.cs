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
        [ForeignKey("TypeDate")]
        public Guid TypeDateId { get; set; }
        public DateTime Date { get; set; }
        public ShiftType ShiftType { get; set; }
        public virtual TypeDate TypeDate { get; set; }
    }
}
