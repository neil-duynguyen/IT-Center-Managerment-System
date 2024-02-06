using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Installment : BaseEntity
    {
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public Guid TraningProgramId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Paid { get;set; }
        public string NotPaid { get; set; }
        public double TotalPrice { get; set; }
        public double CurrentPrice { get; set; }
        public virtual User? User { get; set; }
    }
}
