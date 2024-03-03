using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]/")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("BlogTags")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> BlogTags()
        {
            return Ok(await _tagService.GetBlogTags());
        }

        [HttpGet("SkillTags")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> SkillTags()
        {
            return Ok(await _tagService.GetSkillTags());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Tag(Guid id)
        {
            var tag = await _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostTag(CreateTagViewModel createTagViewModel)
        {
            try
            {
                var result = await _tagService.CreateTag(createTagViewModel);
                if (result)
                {
                    return Ok("Gắn thẻ đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Gắn thẻ đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutTag(UpdateTagViewModel updateTagViewModel)
        {
            try
            {
                var result = await _tagService.UpdateTag(updateTagViewModel);
                if (result)
                {
                    return Ok("Gắn thẻ đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Gắn thẻ đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteTag(Guid tagId)
        {
            try
            {
                var result = await _tagService.DeleteTag(tagId);
                if (result)
                {
                    return Ok("Gắn thẻ đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Gắn thẻ đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
