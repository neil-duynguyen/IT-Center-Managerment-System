using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdviseRequestController : ControllerBase
    {
        private readonly IAdviseRequestService _adviseRequestService;
        public AdviseRequestController(IAdviseRequestService adviseRequestService)
        {
            _adviseRequestService = adviseRequestService;
        }

        [HttpGet("AdviseRequests")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Locations()
        {
            return Ok(await _adviseRequestService.GetAdviseRequests());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Location(Guid id)
        {
            var result = await _adviseRequestService.GetAdviseRequestById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostLocation(CreateAdviseRequestViewModel createAdviseRequestViewModel)
        {
            try
            {
                var result = await _adviseRequestService.CreateAdviseRequest(createAdviseRequestViewModel);
                if (result)
                {
                    return Ok("Yêu cầu tư vấn đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Yêu cầu tư vấn đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutLocation(UpdateAdviseRequestViewModel updateAdviseRequestViewModel)
        {
            try
            {
                var result = await _adviseRequestService.UpdateAdviseRequest(updateAdviseRequestViewModel, x => x.Email, x => x.Phone);
                if (result)
                {
                    return Ok("Yêu cầu tư vấn đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Yêu cầu tư vấn đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            try
            {
                var result = await _adviseRequestService.DeleteAdviseRequest(id);
                if (result)
                {
                    return Ok("Yêu cầu tư vấn đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Yêu cầu tư vấn đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
