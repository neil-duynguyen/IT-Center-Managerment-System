using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.NotificationUserViewModels
{
    public class CreateNotificationUserViewModel
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
    }
}
