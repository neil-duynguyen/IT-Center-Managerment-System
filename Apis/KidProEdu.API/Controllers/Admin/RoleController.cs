using KidProEdu.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> GetRole()
        {
            return Ok(await _roleService.GetRole());
        }
    }
}
