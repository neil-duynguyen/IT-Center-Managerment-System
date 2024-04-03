using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.ChildrenAnswerViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenAnswerController : ControllerBase
    {
        private readonly IChildrenAnswerService _childrenAnswerService;
        public ChildrenAnswerController(IChildrenAnswerService childrenAnswerService)
        {
            _childrenAnswerService = childrenAnswerService;
        }

        [HttpGet("ChildrenAnswers/{childrenId}/{examId}")]
        /*[Authorize(Roles = ("Admin"))]*/

        public async Task<IActionResult> ChildrenAnswers(Guid childrenId, Guid examId)
        {
            return Ok(await _childrenAnswerService.GetChildrenAnswers(childrenId, examId));
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ChildrenAnswerById(Guid id)
        {
            return Ok(await _childrenAnswerService.GetChildrenAnswerById(id));
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostChildrenAnswer(List<CreateChildrenAnswerViewModel> createChildrenAnswerViewModels)
        {
            try
            {
                var result = await _childrenAnswerService.CreateChildrenAnswers(createChildrenAnswerViewModels);
                if (result)
                {
                    return Ok("Câu trả lời đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Câu trả lời đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutChildrenAnswer(UpdateChildrenAnswerViewModel updateChildrenAnswerViewModel)
        {
            try
            {
                var result = await _childrenAnswerService.UpdateChildrenAnswer(updateChildrenAnswerViewModel);
                if (result)
                {
                    return Ok("Câu trả lời đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Câu trả lời đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteChildrenAnswer(Guid id)
        {
            try
            {
                var result = await _childrenAnswerService.DeleteChildrenAnswer(id);
                if (result)
                {
                    return Ok("Câu trả lời đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Câu trả lời đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
