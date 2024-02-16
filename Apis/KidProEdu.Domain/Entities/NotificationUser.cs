using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class NotificationUser : BaseEntity
    {
        [ForeignKey("Notification")]
        public Guid NotificationId { get; set; }
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
