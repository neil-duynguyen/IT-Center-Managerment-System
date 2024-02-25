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

        [HttpGet("StatusOfEquipment")]
        public async Task<IActionResult> StatusOfEquipment()
        {
            List<EnumViewModel> enums = ((StatusOfEquipment[])Enum.GetValues(typeof(StatusOfEquipment))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("CourseType")]
        public async Task<IActionResult> CourseType()
        {
            List<EnumViewModel> enums = ((CourseType[])Enum.GetValues(typeof(CourseType))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }
    }
}
