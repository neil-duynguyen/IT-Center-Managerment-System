using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : Controller
    {
        private readonly ISlotService _slotService;
        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet("Slots")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Slots()
        {
            return Ok(await _slotService.GetSlots());
        }
    }
}

