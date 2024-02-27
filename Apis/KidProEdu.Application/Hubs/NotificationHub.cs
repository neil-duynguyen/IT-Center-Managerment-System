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
        public async Task SendNotificationToUsers(IEnumerable<string> userIds, string message)
        {
            foreach (var userId in userIds)
            {
                await Clients.User(userId).SendAsync("ReceiveNotification", message);
            }
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
