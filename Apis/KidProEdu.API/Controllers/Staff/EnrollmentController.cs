using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
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

        [HttpGet("GetEnrollmentById/{id}")]
        [Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> GetEnrollmentById(Guid Id)
        {
            return Ok(await _enrollmentServices.GetEnrollmentById(Id));
        }

        [HttpPost]
        public async Task<IActionResult> PostEnrollment(CreateEnrollmentViewModel createEnrollmentViewModel)
        {
            try
            {
                var result = await _enrollmentServices.CreateEnrollment(createEnrollmentViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
