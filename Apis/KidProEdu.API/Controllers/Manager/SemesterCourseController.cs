using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.SemesterCourseViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterCourseController : ControllerBase
    {
        private readonly ISemesterCourseService _semesterCourseService;

        public SemesterCourseController(ISemesterCourseService semesterCourseService)
        {
            _semesterCourseService = semesterCourseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(CreateSemesterCourseViewModel createSemesterCourseView)
        {
            try
            {
                return Ok(await _semesterCourseService.AddCourse(createSemesterCourseView));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseBySemesterId(Guid SemesterId)
        {
            return Ok(await _semesterCourseService.GetSemesterCourseById(SemesterId));
        }

        [HttpDelete("DeleteCourseInSemester/{idCourse}")]
        public async Task<IActionResult> DeleteCourseInSemester(Guid idCourse)
        {
            try
            {
                var result = await _semesterCourseService.DeleteSemesterCourse(idCourse);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
