using DocumentFormat.OpenXml.Office2010.Excel;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet("Exams")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Exams()
        {
            return Ok(await _examService.GetExams());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Exam(Guid id)
        {
            var result = await _examService.GetExamById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("ExamByCourse/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ExamByCourse(Guid id)
        {
            var result = await _examService.GetExamsByCourseId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("ExamByClass/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ExamByClass(Guid id)
        {
            var result = await _examService.GetExamsByClassId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("ExamByTestName/{testName}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ExamByTestName(string testName)
        {
            var result = await _examService.GetExamByTestName(testName);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostExam(CreateExamViewModel2 createExamViewModel)
        {
            
            try
            {
                var result = await _examService.CreateExam(createExamViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostExamFinalPractice")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostExamFinalPractice(CreateExamFinalPracticeViewModel createExamViewModel)
        {

            try
            {
                var result = await _examService.CreateExamFinalPractice(createExamViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutExam(UpdateExamViewModel updateExamViewModel)
        {
            try
            {
                var result = await _examService.UpdateExam(updateExamViewModel);
                if (result)
                {
                    return Ok("Bài kiểm tra đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Bài kiểm tra đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            try
            {
                var result = await _examService.DeleteExam(id);
                if (result)
                {
                    return Ok("Bài kiểm tra đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Bài kiểm tra đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
