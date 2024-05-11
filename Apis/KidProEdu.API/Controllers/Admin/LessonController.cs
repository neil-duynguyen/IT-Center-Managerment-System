using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.RatingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("Lessons")]
        public async Task<IActionResult> Lessons()
        {
            return Ok(await _lessonService.GetLessons());
        }

        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> PostLesson(CreateLessonViewModel createLessonViewModel)
        {
            try
            {
                var result = await _lessonService.CreateLesson(createLessonViewModel);
                if (result)
                {
                    return Ok("Bài học đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Bài học đã được tạo thất bại.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]       
        public async Task<IActionResult> Lesson(Guid id)
        {
            var rating = await _lessonService.GetLessonById(id);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpGet("LessonByCourse/{courseId}")]     
        public async Task<IActionResult> LessonByCourseId(Guid courseId)
        {
            var rating = await _lessonService.GetLessonsByCourseId(courseId);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpPut]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> PutLesson(UpdateLessonViewModel updateLessonViewModel)
        {
            try
            {
                var result = await _lessonService.UpdateLesson(updateLessonViewModel);
                if (result)
                {
                    return Ok("Bài học đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Bài học đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            try
            {
                var result = await _lessonService.DeleteLesson(id);
                if (result)
                {
                    return Ok("Bài học đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Bài học đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
