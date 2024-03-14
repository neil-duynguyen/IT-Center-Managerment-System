using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ConfigJobType;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigJobTypeController : ControllerBase
    {
        private readonly IConfigJobTypeService _ConfigJobTypeService;
        public ConfigJobTypeController(IConfigJobTypeService ConfigJobTypeService)
        {
            _ConfigJobTypeService = ConfigJobTypeService;
        }

        [HttpGet("ConfigJobTypes")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ConfigJobTypes()
        {
            return Ok(await _ConfigJobTypeService.GetConfigJobTypes());
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutConfigJobType(UpdateConfigJobTypeViewModel updateConfigJobTypeViewModel)
        {
            try
            {
                var result = await _ConfigJobTypeService.UpdateConfigJobType(updateConfigJobTypeViewModel);
                if (result)
                {
                    return Ok("Cấu hình đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Cấu hình đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
