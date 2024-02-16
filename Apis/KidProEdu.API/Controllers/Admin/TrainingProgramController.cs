using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.TrainingProgramViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]/")]
    [ApiController]
    public class TrainingProgramController : Controller
    {
        private readonly ITrainingProgramService _trainingProgramService;
        public TrainingProgramController(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }

        [HttpGet("TrainingPrograms")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> TrainingPrograms()
        {
            return Ok(await _trainingProgramService.GetTrainingPrograms());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> TrainingProgram(Guid id)
        {
            var TrainingProgram = await _trainingProgramService.GetTrainingProgramById(id);
            if (TrainingProgram == null)
            {
                return NotFound();
            }
            return Ok(TrainingProgram);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostTrainingProgram(CreateTrainingProgramViewModel createTrainingProgramViewModel)
        {
            try
            {
                var result = await _trainingProgramService.CreateTrainingProgram(createTrainingProgramViewModel);
                if (result)
                {
                    return Ok("TrainingProgram đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("TrainingProgram đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutTrainingProgram(UpdateTrainingProgramViewModel updateTrainingProgramViewModel)
        {
            try
            {
                var result = await _trainingProgramService.UpdateTrainingProgram(updateTrainingProgramViewModel, x => x.TrainingProgramName, x => x.TrainingProgramCode);
                if (result)
                {
                    return Ok("TrainingProgram đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("TrainingProgram đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteTrainingProgram(Guid TrainingProgramId)
        {
            try
            {
                var result = await _trainingProgramService.DeleteTrainingProgram(TrainingProgramId);
                if (result)
                {
                    return Ok("TrainingProgram đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("TrainingProgram đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
