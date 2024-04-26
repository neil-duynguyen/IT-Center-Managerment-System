using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel createCourseView)
        {
            try
            {
                return await _courseService.CreateCourseAsync(createCourseView) ? Ok("Khoá học đã được tạo thành công.") : BadRequest("Khoá học đã được tạo thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CourseParent")]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> CreateCourseParent(CreateCourseParentViewModel createCourseParentViewModel)
        {
            try
            {
                return await _courseService.CreateCourseParentAsync(createCourseParentViewModel) ? Ok("Khoá học đã được tạo thành công.") : BadRequest("Khoá học đã được tạo thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> UpdateCourse(UpdateCourseViewModel updateCourseView)
        {
            try
            {
                return await _courseService.UpdateCourseAsync(updateCourseView) ? Ok("Khoá học đã được cập nhật thành công.") : BadRequest("Khoá học đã được cập nhật thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CourseParent")]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> UpdateCourseParent(UpdateCourseParentViewModel updateCourseParentViewModel)
        {
            try
            {
                return await _courseService.UpdateCourseParentAsync(updateCourseParentViewModel) ? Ok("Khoá học đã được cập nhật thành công.") : BadRequest("Khoá học đã được cập nhật thất bại.");
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

        [HttpGet("CoursesInBlog")]
        public async Task<IActionResult> GetAllCourseInBlog()
        {
            return Ok(await _courseService.GetAllCourseInBlog());
        }

        [HttpGet("CoursesSingle")]
        public async Task<IActionResult> GetAllCourseSingle()
        {
            return Ok(await _courseService.GetAllCourseSingle());
        }

        [HttpGet("Courses/{childrenId}")]
        public async Task<IActionResult> GetAllCoursesByChilrenId(Guid childrenId)
        {
            return Ok(await _courseService.GetAllCourseByChildId(childrenId));
        }

        [HttpGet("GetCourseById/{Id}")]
        public async Task<IActionResult> GetCourseById(Guid Id)
        {
            try
            {
                return Ok(await _courseService.GetCourseById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = ("Admin"))]
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
