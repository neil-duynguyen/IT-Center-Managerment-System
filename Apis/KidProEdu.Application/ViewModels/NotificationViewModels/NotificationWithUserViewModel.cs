using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.NotificationViewModels
{
    public class NotificationWithUserViewModel
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Message { get; set; }
    }
}
