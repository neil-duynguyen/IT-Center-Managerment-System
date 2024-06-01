using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = ("Admin, Manager"))]
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

        [HttpPost("BorrowEquipment")]
        //[Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> BorrowEquipment(List<BorrowAutoCategoryEquipmentViewModel> borrowCategoryEquipmentViewModels)
        {
            try
            {
                var result = await _categoryEquipmentService.BorrowCategoryEquipment(borrowCategoryEquipmentViewModels);
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

        [HttpPost("BorrowEquipmentWithStaff")]
        //[Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> BorrowEquipmentWithStaff(List<BorrowCategoryEquipmentViewModel> borrowCategoryEquipmentViewModels)
        {
            try
            {
                var result = await _categoryEquipmentService.BorrowWithStaffCategoryEquipment(borrowCategoryEquipmentViewModels);
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

        [HttpPost("BorrowEquipmentForGoHome")]
        //[Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> BorrowEquipmentForGoHome(List<BorrowForGoHomeCategoryEquipmentViewModel> borrowForGoHomeCategoryEquipmentViewModels)
        {
            try
            {
                var result = await _categoryEquipmentService.BorrowForGoHomeCategoryEquipment(borrowForGoHomeCategoryEquipmentViewModels);
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

        [HttpPost("ReturnEquipment")]
        //[Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> ReturnEquipmentManagement(List<ReturnCategoryEquipmentViewModel> returnCategoryEquipmentViewModels)
        {
            try
            {
                var result = await _categoryEquipmentService.ReturnCategoryEquipment(returnCategoryEquipmentViewModels);
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

        [HttpPost("ReturnEquipmentForHome")]
        //[Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> ReturnEquipmentForHome(List<ReturnCategoryEquipmentViewModel> returnCategoryEquipmentViewModels)
        {
            try
            {
                var result = await _categoryEquipmentService.ReturnForHomeCategoryEquipment(returnCategoryEquipmentViewModels);
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
        [Authorize(Roles = ("Manager"))]
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

        [HttpPut("UpdateQuantityEquipment")]
        [Authorize(Roles = ("Admin, Manager, Staff"))]
        public async Task<IActionResult> UpdateQuantityEquipment(UpdateQuantityCategoryEquipment updateQuantityCategory)
        {
            try
            {
                await _categoryEquipmentService.UpdateQuantityEquipment(updateQuantityCategory);
                return Ok("Cho mượn thành công.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EquipmentReport")]
        public async Task<IActionResult> EquipmentReport()
        {
            try
            {
                return Ok(await _categoryEquipmentService.EquipmentReport());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

