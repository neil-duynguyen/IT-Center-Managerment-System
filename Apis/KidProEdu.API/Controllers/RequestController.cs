using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.RequestViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet("Requests")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Requests()
        {
            try
            {
                return Ok(await _requestService.GetRequests());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Request(Guid id)
        {
            try
            {
                var Request = await _requestService.GetRequestById(id);
                if (Request == null)
                {
                    return NotFound();
                }

                return Ok(Request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetRequestByUser/{id}")]
        [Authorize(Roles = ("Admin, Manager, Staff, Parent, Teacher"))]
        public async Task<IActionResult> GetRequestByUserId(Guid id)
        {
            try
            {
                return Ok(await _requestService.GetRequestByUser(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetRequestByReciever/{id}")]
        [Authorize(Roles = ("Admin, Manager, Staff, Teacher"))]
        public async Task<IActionResult> GetRequestByRecieverId(Guid id)
        {
            try
            {
                return Ok(await _requestService.GetRequestByReceiver(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Manager, Staff, Teacher, Parent"))]
        public async Task<IActionResult> PostRequest(CreateRequestViewModel createRequestViewModel)
        {
            try
            {
                var result = await _requestService.CreateRequest(createRequestViewModel);
                if (result)
                {
                    return Ok("Yêu cầu đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Yêu cầu đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = ("Admin, Manager, Staff, Teacher, Parent"))]
        public async Task<IActionResult> PutRequest(UpdateRequestViewModel updateRequestViewModel)
        {
            try
            {
                var result = await _requestService.UpdateRequest(updateRequestViewModel);
                if (result)
                {
                    return Ok("Yêu cầu đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Yêu cầu đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = ("Admin, Manager, Staff, Teacher, Parent"))]
        public async Task<IActionResult> DeleteRequest(Guid RequestId)
        {
            try
            {
                var result = await _requestService.DeleteRequest(RequestId);
                if (result)
                {
                    return Ok("Yêu cầu đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Yêu cầu đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ChangeStatusRequest")]
        [Authorize(Roles = ("Admin, Manager, Staff, Teacher"))]
        public async Task<IActionResult> ChangeStatusRequest(ChangeStatusRequestViewModel changeStatusRequestViewModel)
        {
            try
            {
                var result = await _requestService.ChangeStatusRequest(changeStatusRequestViewModel);
                if (result)
                {
                    return Ok("Yêu cầu đã được cập nhật trạng thái thành công.");
                }
                else
                {
                    return BadRequest("Yêu cầu cập nhật trạng thái thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
