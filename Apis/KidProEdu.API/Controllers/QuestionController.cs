using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.QuestionViewModels;
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
        
        [HttpGet("GetByLesson/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> GetQuestionByLesson(Guid id)
        {
            var question = await _questionService.GetQuestionByLesson(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
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
        /*[Authorize(Roles = ("Admin"))]*/
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
        /*[Authorize(Roles = ("Admin"))]*/
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
