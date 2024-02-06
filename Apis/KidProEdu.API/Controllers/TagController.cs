using KidProEdu.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("Tags")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Tags()
        {
            return Ok(await _tagService.GetTags());
        }
    }
}
