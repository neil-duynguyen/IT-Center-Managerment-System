using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.NotificationViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("Notifications")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Notifications()
        {
            return Ok(await _notificationService.GetNotifications());
        }

        [HttpGet("Notifications/{userId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> NotificationsByUserId(Guid userId)
        {
            return Ok(await _notificationService.GetNotificationsByUserId(userId));
        }

        /*[HttpPost]
        *//*[Authorize(Roles = ("Admin"))]*//*
        public async Task<IActionResult> PostNotification(CreateNotificationViewModel createNotificationViewModel, Guid[] userId)
        {
            try
            {
                var result = await _notificationService.CreateNotification(createNotificationViewModel, userId);
                if (result)
                {
                    return Ok("Thông báo được tạo thành công.");
                }
                else
                {
                    return BadRequest("Thông báo đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}
