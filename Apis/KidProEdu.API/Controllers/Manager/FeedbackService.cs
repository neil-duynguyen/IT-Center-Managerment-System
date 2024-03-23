using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.FeedBackViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackService : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackService(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("Feedbacks")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Feedbacks()
        {
            return Ok(await _feedbackService.GetFeedbacks());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Feedback(Guid id)
        {
            var result = await _feedbackService.GetFeedbackById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("FeedbackByUserId/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> FeedbackByUserId(Guid id)
        {
            var result = await _feedbackService.GetFeedbackByUserId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("FeedbackByClassId/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> FeedbackByClassId(Guid id)
        {
            var result = await _feedbackService.GetFeedbackByClassId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostFeedback(CreateFeedBackViewModel createFeedBackViewModel)
        {
            try
            {
                var result = await _feedbackService.CreateFeedback(createFeedBackViewModel);
                if (result)
                {
                    return Ok("Đánh giá đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Đánh giá đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutFeedback(UpdateFeedBackViewModel updateFeedBackViewModel)
        {
            try
            {
                var result = await _feedbackService.UpdateFeedback(updateFeedBackViewModel);
                if (result)
                {
                    return Ok("Đánh giá đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Đánh giá đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedback(id);
                if (result)
                {
                    return Ok("Đánh giá đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Đánh giá đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
