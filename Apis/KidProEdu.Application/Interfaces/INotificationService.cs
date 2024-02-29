using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.NotificationViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface INotificationService
    {
        Task<List<Notification>> GetNotifications();

        Task<List<NotificationWithUserViewModel>> GetNotificationsByUserId(Guid userId);
        Task<bool> CreateNotification(CreateNotificationViewModel createNotificationViewModel, Guid[] userId);
    }
}
