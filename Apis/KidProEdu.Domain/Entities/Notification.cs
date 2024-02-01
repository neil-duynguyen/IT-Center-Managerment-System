using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public DateTime? Date { get; set; }
        public string? Message { get; set; }

        public IList<NotificationUser> NotificationUser { get; set; }
    }
}
