using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Test : BaseEntity
    {
        [ForeignKey("AdviseRequest")]
        public Guid AdviseRequestId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Question { get; set; }
        public string Answers { get; set; }
        public double Score { get; set; }
        public StatusTest Status { get; set; }
        public virtual AdviseRequest AdviseRequest { get; set; }
    }
}
