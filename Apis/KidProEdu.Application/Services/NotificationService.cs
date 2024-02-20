using AutoMapper;
using FluentValidation;
using KidProEdu.Application.Hubs;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Equipments;
using KidProEdu.Application.Validations.Notifications;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.NotificationUserViewModels;
using KidProEdu.Application.ViewModels.NotificationViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<bool> CreateNotification(CreateNotificationViewModel createNotificationViewModel)
        {
            try
            {
                var validator = new CreateNotificationViewModelValidator();
                var validationResult = validator.Validate(createNotificationViewModel);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }
                createNotificationViewModel.Id = Guid.NewGuid();
                createNotificationViewModel.Date = DateTime.Now;
                createNotificationViewModel.Message = createNotificationViewModel.Message;
                if (createNotificationViewModel.CreateNotificationUserViewModels == null)
                {
                    createNotificationViewModel.CreateNotificationUserViewModels = new List<CreateNotificationUserViewModel>();
                }

                foreach (var notificationUsers in createNotificationViewModel.CreateNotificationUserViewModels)
                {
                    notificationUsers.NotificationId = createNotificationViewModel.Id;
                    notificationUsers.UserId = notificationUsers.UserId;
                }

                var newNotification = _mapper.Map<Notification>(createNotificationViewModel);

                await _unitOfWork.NotificationRepository.AddAsync(newNotification);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    foreach (var notificationUser in createNotificationViewModel.CreateNotificationUserViewModels)
                    {
                        await _hubContext.Clients.User(notificationUser.UserId.ToString()).SendAsync("ReceiveNotification", createNotificationViewModel.Message);
                    }

                    return true;
                }
                else
                {
                    throw new Exception("Tạo thông báo thất bại");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Notification>> GetNotifications()
        {
            return _unitOfWork.NotificationRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).ToList(); ;
        }
    }
}
