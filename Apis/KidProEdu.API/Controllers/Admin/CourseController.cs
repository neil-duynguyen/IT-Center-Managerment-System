﻿using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> PostCourse(CreateCourseViewModel createCourseView) 
        {
            try
            {
                return await _courseService.CreateCourseAsync(createCourseView) ? Ok("Course đã được tạo thành công.") : BadRequest("Course đã được tạo thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Courses")]
        public async Task<IActionResult> GetAllCourse()
        {
            return Ok(await _courseService.GetAllCourse());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            try
            {
                return await _courseService.DeleteCourseAsync(courseId) ? Ok("Xoá Course thành công") : BadRequest("Xóa Course thất bại");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
