using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.TrainingProgramCategoryViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramCategoryController : ControllerBase
    {
        private readonly ITrainingProgramCategoryService _trainingProgramCategoryService;
        public TrainingProgramCategoryController(ITrainingProgramCategoryService trainingProgramCategoryService)
        {
            _trainingProgramCategoryService = trainingProgramCategoryService;
        }

        [HttpGet("TrainingProgramCategories")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> TrainingProgramCategories()
        {
            return Ok(await _trainingProgramCategoryService.GetTrainingProgramCategories());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> TrainingProgramCategory(Guid id)
        {
            var trainingProgramCategory = await _trainingProgramCategoryService.GetTrainingProgramCategoryById(id);
            if (trainingProgramCategory == null)
            {
                return NotFound();
            }
            return Ok(trainingProgramCategory);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostTrainingProgramCategory(CreateTrainingProgramCategoryViewModel createTrainingProgramCategoryViewModel)
        {
            try
            {
                var result = await _trainingProgramCategoryService.CreateTrainingProgramCategory(createTrainingProgramCategoryViewModel);
                if (result)
                {
                    return Ok("Chương trình đào tạo đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Chương trình đào tạo đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutTrainingProgramCategory(UpdateTrainingProgramCategoryViewModel updateTrainingProgramCategoryViewModel)
        {
            try
            {
                var result = await _trainingProgramCategoryService.UpdateTrainingProgramCategory(updateTrainingProgramCategoryViewModel);
                if (result)
                {
                    return Ok("Chương trình đào tạo đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Chương trình đào tạo đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteTrainingProgramCategory(Guid id)
        {
            try
            {
                var result = await _trainingProgramCategoryService.DeleteTrainingProgramCategory(id);
                if (result)
                {
                    return Ok("Chương trình đào tạo đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Chương trình đào tạo đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
