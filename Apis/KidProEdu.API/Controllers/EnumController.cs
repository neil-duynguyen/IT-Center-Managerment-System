using KidProEdu.Application.ViewModels;
using KidProEdu.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        [HttpGet("StatusOfRoom")]
        public async Task<IActionResult> StatusOfRoom()
        {
            List<EnumViewModel> enums = ((StatusOfRoom[])Enum.GetValues(typeof(StatusOfRoom))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }
    }
}
