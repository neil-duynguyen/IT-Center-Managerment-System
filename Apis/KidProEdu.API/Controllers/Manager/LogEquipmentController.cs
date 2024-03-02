using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LogEquipmentViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogEquipmentController : ControllerBase
    {
        private readonly ILogEquipmentService _logEquipmentService;
        public LogEquipmentController(ILogEquipmentService logEquipmentService)
        {
            _logEquipmentService = logEquipmentService;
        }

        [HttpGet("LogEquipments")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipments()
        {
            return Ok(await _logEquipmentService.GetLogEquipments());
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostLogEquipment(CreateLogEquipmentViewModel createLogEquipmentViewModel)
        {
            try
            {
                var result = await _logEquipmentService.CreateLogEquipment(createLogEquipmentViewModel);
                if (result)
                {
                    return Ok("Tài liệu đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Tài liệu đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipment(Guid id)
        {
            var result = await _logEquipmentService.GetLogEquipmentById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentByRoomId/{roomId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipmentByRoomId(Guid roomId)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByRoomId(roomId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentByUserId/{userId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipmentByUserId(Guid userId)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByUserId(userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}
