using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Attendances()
        {
            return Ok(await _attendanceService.GetAttendances());
        }

        [HttpGet("AttendanceByScheduleId/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> AttendanceByScheduleId(Guid id)
        {
            return Ok(await _attendanceService.GetAttendanceByScheduleId(id));
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutAttendance(List<UpdateAttendanceViewModel> updateAttendanceViewModels)
        {
            try
            {
                var result = await _attendanceService.UpdateAttendances(updateAttendanceViewModels);
                if (result)
                {
                    return Ok("Kĩ năng đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Kĩ năng đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
