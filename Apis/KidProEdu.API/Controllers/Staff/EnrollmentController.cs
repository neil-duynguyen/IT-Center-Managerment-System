using KidProEdu.Application.Interfaces;
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

        [HttpGet("GetEnrollmentById")]
        [Authorize(Roles = ("Manager"))]
        public async Task<IActionResult> GetEnrollmentById(Guid Id)
        {
            return Ok(await _enrollmentServices.GetEnrollmentById(Id));
        }
    }
}
