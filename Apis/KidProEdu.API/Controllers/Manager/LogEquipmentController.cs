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
                    return Ok("Nhật kí thiết bị đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Nhật kí thiết bị đã được tạo thất bại.");
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

        [HttpGet("LogEquipmentByName/{name}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipmentByName(string name)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByName(name);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentByCode/{code}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipmentByCode(string code)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByCode(code);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentByEquipmentId/{equipmentId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipmentByEquipmentId(Guid equipmentId)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByEquipmentId(equipmentId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteLogEquipment(Guid id)
        {
            try
            {
                var result = await _logEquipmentService.DeleteLogEquipment(id);
                if (result)
                {
                    return Ok("Nhật kí thiết bị đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Nhật kí thiết bị đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutLogEquipment(UpdateLogEquipmentViewModel updateLogEquipmentViewModel)
        {
            try
            {
                var result = await _logEquipmentService.UpdateLogEquipment(updateLogEquipmentViewModel);
                if (result)
                {
                    return Ok("Nhật kí thiết bị đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Nhật kí thiết bị đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
