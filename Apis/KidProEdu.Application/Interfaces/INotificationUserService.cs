using KidProEdu.Application.ViewModels.NotificationUserViewModels;
using KidProEdu.Application.ViewModels.NotificationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface INotificationUserService
    {
        Task<bool> CreateNotificationUser(CreateNotificationUserViewModel createNotificationUserViewModel);
    }
}
