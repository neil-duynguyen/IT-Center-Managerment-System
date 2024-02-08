using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryEquipmentController : ControllerBase
    {
        private readonly ICategoryEquipmentService _categoryEquipmentService;
        public CategoryEquipmentController(ICategoryEquipmentService categoryEquipmentService)
        {
            _categoryEquipmentService = categoryEquipmentService;
        }

        [HttpGet("CategoryEquipments")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> CategoryEquipments()
        {
            return Ok(await _categoryEquipmentService.GetCategoryEquipments());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> CategoryEquipment(Guid id)
        {
            var categoryEquipment = await _categoryEquipmentService.GetCategoryEquipmentById(id);
            if (categoryEquipment == null)
            {
                return NotFound();
            }
            return Ok(categoryEquipment);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostCategoryEquipment(CreateCategoryEquipmentViewModel createCategoryEquipmentViewModel)
        {
            try
            {
                var result = await _categoryEquipmentService.CreateCategoryEquipment(createCategoryEquipmentViewModel);
                if (result)
                {
                    return Ok("Danh mục thiết bị đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Danh mục thiết bị đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutCategoryEquipment(UpdateCategoryEquipmentViewModel updateCategoryEquipmentViewModel)
        {
            try
            {
                var result = await _categoryEquipmentService.UpdateCategoryEquipment(updateCategoryEquipmentViewModel);
                if (result)
                {
                    return Ok("Danh mục thiết bị đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Danh mục thiết bị đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteCategoryEquipment(Guid id)
        {
            try
            {
                var result = await _categoryEquipmentService.DeleteCategoryEquipment(id);
                if (result)
                {
                    return Ok("Danh mục thiết bị đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Danh mục thiết bị đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

