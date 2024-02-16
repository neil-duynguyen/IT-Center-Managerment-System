using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }
        public Guid RecipientId { get; set; }
        [ForeignKey("Class")]
        public Guid? ClassId { get; set; }
        public string? Messages {  get; set; }
        public string? Stars { get; set; }
        public virtual UserAccount UserAccount { get; set; }

        public virtual Class? Class { get; set; }
        
    }
}
