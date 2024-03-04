using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]/")]
    [ApiController]
    public class SemesterController : Controller
    {
        private readonly ISemesterService _semesterService;
        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpGet("Semesters")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Semesters()
        {
            return Ok(await _semesterService.GetSemesters());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Semester(Guid id)
        {
            var Semester = await _semesterService.GetSemesterById(id);
            if (Semester == null)
            {
                return NotFound();
            }
            return Ok(Semester);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostSemester()
        {
            try
            {
                var result = await _semesterService.CreateSemester();
                if (result)
                {
                    return Ok("Semester đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Semester đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPut]
        *//*[Authorize(Roles = ("Admin"))]*//*
        public async Task<IActionResult> PutSemester(UpdateSemesterViewModel updateSemesterViewModel)
        {
            try
            {
                var result = await _semesterService.UpdateSemester(updateSemesterViewModel, x => x.SemesterName, x => x.StartDate);
                if (result)
                {
                    return Ok("Semester đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Semester đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        *//*[Authorize(Roles = ("Admin"))]*//*
        public async Task<IActionResult> DeleteSemester(Guid id)
        {
            try
            {
                var result = await _semesterService.DeleteSemester(id);
                if (result)
                {
                    return Ok("Semester đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Semester đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        [HttpPut("ChangeStatusSemester")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ChangeStatusSemester(Guid id)
        {
            try
            {
                var result = await _semesterService.ChangeStatusSemester(id);
                if (result)
                {
                    return Ok("Semester cập nhật trạng thái thành công.");
                }
                else
                {
                    return BadRequest("Semester cập nhật trạng thái thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
