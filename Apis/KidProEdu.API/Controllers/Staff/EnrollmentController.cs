using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentServices _enrollmentServices;

        public EnrollmentController(IEnrollmentServices enrollmentServices)
        {
            _enrollmentServices = enrollmentServices;
        }

        [HttpGet("Enrollments")]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> GetEnrollment()
        {
            return Ok(await _enrollmentServices.GetEnrollment());
        }

        [HttpGet("GetEnrollmentByStaffId/{id}")]
        [Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> GetEnrollmentById(Guid Id)
        {
            return Ok(await _enrollmentServices.GetEnrollmentById(Id));
        }

        [HttpGet("GetEnrollmentByClass/{classId}")]
        //[Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> GetEnrollmentByClass(Guid classId)
        {
            return Ok(await _enrollmentServices.GetEnrollmentsByClassId(classId));
        }

        [HttpPost]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> PostEnrollment(CreateEnrollmentViewModel createEnrollmentViewModel)
        {
            try
            {
                var result = await _enrollmentServices.CreateEnrollment(createEnrollmentViewModel);
                if (result.Any())
                {
                    return BadRequest(string.Join("\n", result));
                }
                else
                {
                    return Ok("Thêm danh sách học sinh vào lớp thành công");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> PutEnrollment(UpdateEnrollmentViewModel updateEnrollmentViewModel)
        {
            try
            {
                var result = await _enrollmentServices.UpdateEnrollment(updateEnrollmentViewModel);
                if (result)
                {
                    return Ok("Tham gia đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Tham gia đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EnrollmentStudying")]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> PutEnrollmentStudying(UpdateEnrollmentViewModel updateEnrollmentViewModel)
        {
            try
            {
                var result = await _enrollmentServices.UpdateEnrollmentStudying(updateEnrollmentViewModel);
                if (result)
                {
                    return Ok("Tham gia đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Tham gia đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> DeleteEnrolled(Guid classId, Guid childId)
        {
            try
            {
                var result = await _enrollmentServices.DeleteEnrollment(classId, childId);
                if (result)
                {
                    return Ok("Tham gia lớp học đã được xóa thành công");
                }
                else
                {
                    return BadRequest("Tham gia lớp học đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ImportExcelFile")]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> ImportExcelFile(IFormFile formFile)
        {
            try
            {
                var result = await _enrollmentServices.ImportExcelFile(formFile);
                return Ok("Thêm danh sách học sinh vào lớp thành công");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
