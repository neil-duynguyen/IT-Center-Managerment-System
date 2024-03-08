using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildrenService _childrenService;

        public ChildrenController(IChildrenService childrenService)
        {
            _childrenService = childrenService;
        }

        [HttpPost]
        /*[Authorize(Roles = ("Staff"))]*/
        public async Task<IActionResult> CreateChildren(CreateChildrenViewModel createChildrenViewModel) 
        {
            try
            {
                var result = await _childrenService.CreateChildren(createChildrenViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                
            }
        }


        [HttpPut]
        /*[Authorize(Roles = ("Staff"))]*/
        public async Task<IActionResult> UpdateChildren(UpdateChildrenViewModel updateChildrenViewModel)
        {
            try
            {
                var result = await _childrenService.UpdateChildren(updateChildrenViewModel);
                return Ok("Cập nhật trẻ thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetChildrensByStaffId")]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> GetChildrensByStaffId()
        {
            try
            {
                var result = await _childrenService.GetChildrensByStaffId();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
