using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
