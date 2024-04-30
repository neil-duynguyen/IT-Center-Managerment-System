using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AdviseRequests()
        {
            try
            {
                return Ok(await _adviseRequestService.GetAdviseRequests());
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> AdviseRequest(Guid id)
        {
            try
            {
                var result = await _adviseRequestService.GetAdviseRequestById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
        
        [HttpGet("GetTestDate/{testDate}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> TestDate(DateTime testDate)
        {
            try
            {
                var result = await _adviseRequestService.GetAdviseRequestByTestDate(testDate);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin, Manager, Staff"))]*/
        public async Task<IActionResult> PostAdviseRequest(CreateAdviseRequestViewModel createAdviseRequestViewModel)
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
        [Authorize(Roles = ("Admin, Manager, Staff, Parent"))]
        public async Task<IActionResult> PutAdviseRequest(UpdateAdviseRequestViewModel updateAdviseRequestViewModel)
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
        [Authorize(Roles = ("Admin, Manager, Staff, Parent"))]
        public async Task<IActionResult> DeleteAdviseRequest(Guid id)
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

        [HttpGet("GetAdviseRequestByUserId/{id}")]
        public async Task<IActionResult> GetAdviseRequestByUserId(Guid id)
        {
            try
            {
                var listRequest = await _adviseRequestService.GetAdviseRequestByUserId(id);

                return Ok(listRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
