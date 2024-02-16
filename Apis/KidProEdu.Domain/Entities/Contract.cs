using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class Contract : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }

        [ForeignKey("ConfigJobType")]
        public Guid ConfigJobTypeId { get; set; }
        public string ContractCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Job { get; set; }
        public string File { get; set; }
        public StatusOfContract StatusOfContract { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ConfigJobType ConfigJobType { get; set; }

    }
}
