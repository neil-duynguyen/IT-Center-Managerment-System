using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class DivisionUserAccount : BaseEntity
    {
        [ForeignKey("Division")]
        public Guid DivisionId { get; set; }
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public virtual Division Division { get; set; }
        public virtual UserAccount UserAccount { get; set; }

    }
}
