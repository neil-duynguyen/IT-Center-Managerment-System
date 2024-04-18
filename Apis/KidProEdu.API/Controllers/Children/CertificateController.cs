using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.CertificateViewModel;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Children
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> CreateCertificate(List<CreateCertificateViewModel> certificate)
        {
            try
            {
                var result = await _certificateService.CreateCertificate(certificate);
                if (result)
                {
                    return Ok("Chứng chỉ đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Chứng chỉ đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
