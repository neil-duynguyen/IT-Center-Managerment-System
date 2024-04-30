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
        [Authorize(Roles = ("Staff"))]
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
        [Authorize(Roles = ("Staff, Parent"))]
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

        [HttpPut("/ChildrenReserve")]
        //[Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> ChildrenReserve(ChildrenReserveViewModel childrenReserveViewModel)
        {
            try
            {
                var result = await _childrenService.ChildrenReserve(childrenReserveViewModel);
                return Ok("Cập nhật bảo lưu thành công.");
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

        [HttpGet("ChildrensByOutOfClassId")]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> ChildrensByClassId(Guid classId)
        {
            try
            {
                var result = await _childrenService.GetListChildrenByOutClassId(classId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetChildrenByParentId/{id}")]
        public async Task<IActionResult> GetChildrenByParentId(Guid id)
        {
            try
            {
                var result = await _childrenService.GetChildrenByParentId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetChildrenById/{id}")]
        public async Task<IActionResult> GetChildrenById(Guid id)
        {
            try
            {
                return Ok(await _childrenService.GetChildrenById(id));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CourseSuggestions/{childrenId}")]
        public async Task<IActionResult> CourseSuggestions(Guid childrenId)
        {
            try
            {
                return Ok(await _childrenService.CourseSuggestions(childrenId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
