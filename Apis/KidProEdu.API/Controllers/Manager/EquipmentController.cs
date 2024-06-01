using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;
        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet("Equipments")]
        [Authorize(Roles = ("Admin, Manager, Staff"))]
        public async Task<IActionResult> Equipments()
        {
            return Ok(await _equipmentService.GetEquipments());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = ("Admin, Manager, Staff"))]
        public async Task<IActionResult> Equipment(Guid id)
        {
            var equipment = await _equipmentService.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }
            return Ok(equipment);
        }

        [HttpGet("Equipments/{name}")]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> EquipmentByName(string name)
        {
            var equipments = await _equipmentService.GetListEquipmentByName(name);
            if (equipments == null)
            {
                return NotFound();
            }
            return Ok(equipments);
        }

        [HttpGet("EquipmentsByStatus/{status}")]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> EquipmentsByStatus(StatusOfEquipment status)
        {
            try
            {
                var equipments = await _equipmentService.GetEquipmentByStatus(status);
                if (equipments == null)
                {
                    return NotFound();
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EquipmentsByCategoryId/{cateId}")]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> EquipmentsByCategoryId(Guid cateId)
        {
            try
            {
                var equipments = await _equipmentService.GetListEquipmentByCateId(cateId);
                if (equipments == null)
                {
                    return NotFound();
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EquipmentsByCategoryIdAndStatus/{cateId}")]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> EquipmentsByCategoryIdAndStatus(Guid cateId, StatusOfEquipment status)
        {
            try
            {
                var equipments = await _equipmentService.GetListEquipmentByCateIdAndStatus(cateId, status);
                if (equipments == null)
                {
                    return NotFound();
                }
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> PostEquipment(CreateEquipmentViewModel createEquipmentViewModel)
        {
            try
            {
                var result = await _equipmentService.CreateEquipment(createEquipmentViewModel);
                if (result)
                {
                    return Ok("Thiết bị đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Thiết bị đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EquipmentBorrowedManagement")]
        [Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> EquipmentBorrowedManagement(EquipmentBorrowedManagementViewModel equipmentBorrowedManagementViewModel)
        {
            try
            {
                var result = await _equipmentService.EquipmentBorrowedManagement(equipmentBorrowedManagementViewModel);
                if (result)
                {
                    return Ok("Mượn thiết bị thành công.");
                }
                else
                {
                    return BadRequest("Mượn thiết bị thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EquipmentRepairManagement")]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> EquipmentRepairManagement(EquipmentRepairManagementViewModel equipmentRepairManagementViewModel)
        {
            try
            {
                var result = await _equipmentService.EquipmentRepairManagement(equipmentRepairManagementViewModel);
                if (result)
                {
                    return Ok("Mang thiết bị đi bảo dưỡng thành công.");
                }
                else
                {
                    return BadRequest("Mang thiết bị đi bảo dưỡng thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EquipmentReturnedManagement")]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> EquipmentReturnedManagement(EquipmentReturnedManagementViewModel equipmentReturnedManagementViewModel)
        {
            try
            {
                var result = await _equipmentService.EquipmentReturnedManagement(equipmentReturnedManagementViewModel);
                if (result)
                {
                    return Ok("Trả thiết bị thành công.");
                }
                else
                {
                    return BadRequest("Trả thiết bị thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> PutEquipment(UpdateEquipmentViewModel updateEquipmentViewModel)
        {
            try
            {
                var result = await _equipmentService.UpdateEquipment(updateEquipmentViewModel);
                if (result)
                {
                    return Ok("Thiết bị đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Thiết bị đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> DeleteEquipment(Guid id)
        {
            try
            {
                var result = await _equipmentService.DeleteEquipment(id);
                if (result)
                {
                    return Ok("Thiết bị đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Thiết bị đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GettEquipmentByDate/{date}")]
        [Authorize(Roles = ("Admin, Manager, Staff"))]
        public async Task<IActionResult> GetEquipmentByDate(DateOnly date)
        {
            try
            {
                var result = await _equipmentService.GetEquipmentByDate(date);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("Không có lớp cần chuẩn bị.");
            }
        }

        [HttpGet("GetEquipmentByProgress/{classId}/{progress}")]
        [Authorize(Roles = ("Admin, Manager, Staff"))]
        public async Task<IActionResult> GetEquipmentByProgress(Guid classId, int progress)
        {
            try
            {
                var result = await _equipmentService.GetEquipmentByProgress(classId, progress);
                return Ok(result);

            }
            catch (Exception)
            {
                return NotFound("Không có thiết bị cần chuẩn bị.");
            }
        }

        [HttpGet("ExportExcelFile/{date}")]
        public async Task<IActionResult> ExportExcelFile(DateOnly date) => File(await _equipmentService.ExportExcelFileAsync(date),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "PrepareEquipment.xlsx");

        [HttpPost("PostListEquipment/{quantity}")]
        [Authorize(Roles = ("Admin, Manager, Staff"))]
        public async Task PostListEquipment(CreateEquipmentViewModel createEquipmentViewModel, int quantity)
        {
            await _equipmentService.CreateListEquipment(createEquipmentViewModel, quantity);
        }
    }
}
