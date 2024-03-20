using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillCertificateController : ControllerBase
    {
        private readonly ISkillCertificateService _skillCertificateService;
        public SkillCertificateController(ISkillCertificateService skillCertificateService)
        {
            _skillCertificateService = skillCertificateService;
        }

        [HttpGet("SkillCertificates")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> SkillCertificates()
        {
            return Ok(await _skillCertificateService.GetSkillCertificates());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> SkillCertificate(Guid id)
        {
            var result = await _skillCertificateService.GetSkillCertificateById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("SkillCertificateByUser/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> SkillCertificateByUser(Guid id)
        {
            var result = await _skillCertificateService.GetListSkillCertificatesByUserAccountId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostSkillCertificate(CreateSkillCertificateViewModel createSkillCertificateViewModel)
        {
            try
            {
                var result = await _skillCertificateService.CreateSkillCertificate(createSkillCertificateViewModel);
                if (result)
                {
                    return Ok("Liên kết đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Liên kết đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutSkillCertificate(UpdateSkillCertificateViewModel updateSkillCertificateViewModel)
        {
            try
            {
                var result = await _skillCertificateService.UpdateSkillCertificate(updateSkillCertificateViewModel);
                if (result)
                {
                    return Ok("Liên kết đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Liên kết đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteSkillCertificate(Guid id)
        {
            try
            {
                var result = await _skillCertificateService.DeleteSkillCertificate(id);
                if (result)
                {
                    return Ok("Liên kết đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Liên kết đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
