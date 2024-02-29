using KidProEdu.Application.ViewModels.NotificationUserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.NotificationViewModels
{
    public class CreateNotificationViewModel
    {
        //public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Message { get; set; }
        //public virtual ICollection<CreateNotificationUserViewModel> CreateNotificationUserViewModels { get; set; }
    }
}
