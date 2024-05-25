using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LogEquipmentViewModels;
using KidProEdu.Domain.Enums;
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

        [HttpGet("LogEquipmentsByStatus/{statusOfEquipment}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> LogEquipmentsByStatus(StatusOfEquipment statusOfEquipment)
        {
            return Ok(await _logEquipmentService.GetLogEquipmentByStatus(statusOfEquipment));
        }

        /*[HttpPost]
        [Authorize(Roles = ("Admin"))]
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
        }*/

        [HttpGet("{id}")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> LogEquipment(Guid id)
        {
            var result = await _logEquipmentService.GetLogEquipmentById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentsByRoom/{roomId}")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> LogEquipmentByRoomId(Guid roomId)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByRoomId(roomId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentsByCateEquipmentId/{cateId}")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> LogEquipmentsByCateEquipmentId(Guid cateId)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByCateEquipmentId(cateId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentsByUser/{userId}")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> LogEquipmentByUserId(Guid userId)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByUserId(userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentsByName/{name}")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> LogEquipmentByName(string name)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByName(name);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentsByCode/{code}")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> LogEquipmentByCode(string code)
        {
            var result = await _logEquipmentService.GetLogEquipmentsByCode(code);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet()]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> LogEquipments()
        {
            var result = await _logEquipmentService.GetLogEquipments();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("LogEquipmentsByEquipment/{equipmentId}")]
        //[Authorize(Roles = ("Admin"))]
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
        //[Authorize(Roles = ("Admin"))]
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

        /*[HttpPut]
        [Authorize(Roles = ("Admin"))]
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
        }*/

    }
}
