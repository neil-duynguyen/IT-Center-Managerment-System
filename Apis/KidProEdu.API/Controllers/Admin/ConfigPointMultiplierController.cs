using KidProEdu.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigPointMultiplierController : ControllerBase
    {
        private readonly IConfigPointMultiplierService _ConfigPointMultiplierService;
        public ConfigPointMultiplierController(IConfigPointMultiplierService ConfigPointMultiplierService)
        {
            _ConfigPointMultiplierService = ConfigPointMultiplierService;
        }

        [HttpGet("ConfigPointMultipliers")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ConfigPointMultipliers()
        {
            return Ok(await _ConfigPointMultiplierService.GetConfigPointMultipliers());
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutConfigPointMultiplier(Guid id, double multiplier)
        {
            try
            {
                var result = await _ConfigPointMultiplierService.UpdateConfigPointMultiplier(id, multiplier);
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
