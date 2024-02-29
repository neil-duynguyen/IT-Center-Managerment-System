using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.NotificationUserViewModels;
using KidProEdu.Application.ViewModels.NotificationViewModels;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public NotificationHub(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task SendNotificationToUsers(IEnumerable<string> userIds, string message)
        {
            foreach (var userId in userIds)
            {
                await Clients.User(userId).SendAsync("ReceiveNotification", message);
            }
        }

        public async Task SendMessage(Guid[] user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            CreateNotificationViewModel vm = new()
            {
                Date = _currentTime.GetCurrentTime(),
                Message = message
            };

            NotificationService ns = new NotificationService(_unitOfWork, _currentTime, _claimsService, _mapper);
            await ns.CreateNotification(vm, user);


        }
    }
}
