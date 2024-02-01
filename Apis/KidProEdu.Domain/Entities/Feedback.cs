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
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [ForeignKey("User")]
        public Guid RecipientId { get; set; }
        [ForeignKey("Class")]
        public Guid? ClassId { get; set; }
        public string? Messages {  get; set; }
        public string? Stars { get; set; }
        

        public virtual Class Class { get; set; }
        public virtual User User { get; set; }
        public virtual User Recipient { get; set; }
    }
}
