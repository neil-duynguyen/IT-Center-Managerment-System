using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("Questions")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Questions()
        {
            return Ok(await _questionService.GetQuestions());
        }

        [HttpGet("QuestionsByType")]
        [Authorize(Roles = ("Admin, Manager, Staff, Teacher"))]
        public async Task<IActionResult> QuestionsByType(QuestionType type)
        {
            return Ok(await _questionService.GetQuestionsByType(type));
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Question(Guid id)
        {
            var question = await _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost("CreateTest")]
        [Authorize(Roles = ("Admin, Manager, Staff, Teacher, Parent"))]
        public async Task<IActionResult> CreateTest(List<CreateExamViewModel> createExamViewModels)
        {
            var question = await _questionService.CreateTest(createExamViewModels);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost("CreateTestEntry")]
        [Authorize(Roles = ("Admin, Manager, Staff, Parent"))]
        public async Task<IActionResult> CreateTestEntry(CreateExamEntryViewModel createExamEntryViewModel)
        {
            try
            {
                var question = await _questionService.CreateTestEntry(createExamEntryViewModel);
                if (question == null)
                {
                    return BadRequest("Không tìm thấy danh sách câu hỏi");
                }
                return Ok(question);
            }catch(Exception ex) { 
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> PostQuestion(CreateQuestionViewModel[] createQuestionViewModel)
        {
            try
            {
                var result = await _questionService.CreateQuestion(createQuestionViewModel);
                if (result)
                {
                    return Ok("Câu hỏi đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Câu hỏi đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> PutQuestion(UpdateQuestionViewModel updateQuestionViewModel)
        {
            try
            {
                var result = await _questionService.UpdateQuestion(updateQuestionViewModel);
                if (result)
                {
                    return Ok("Câu hỏi đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Câu hỏi đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = ("Admin, Manager"))]
        public async Task<IActionResult> DeleteQuestion(Guid QuestionId)
        {
            try
            {
                var result = await _questionService.DeleteQuestion(QuestionId);
                if (result)
                {
                    return Ok("Câu hỏi đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Câu hỏi đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
