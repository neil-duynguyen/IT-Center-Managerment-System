using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.BlogTagViewModels;
using KidProEdu.Application.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogTagController : ControllerBase
    {
        private readonly IBlogTagService _blogTagService;
        public BlogTagController(IBlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostBlog(CreateBlogTagViewModel createBlogTagViewModel)
        {
            try
            {
                var result = await _blogTagService.CreateBlogTag(createBlogTagViewModel);
                if (result)
                {
                    return Ok("Liên kết đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Liên kết đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BlogTags")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> BlogTags()
        {
            return Ok(await _blogTagService.GetBlogTags());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> BlogTag(Guid id)
        {
            var blog = await _blogTagService.GetBlogTagById(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }
      
        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutBlogTag(UpdateBlogTagViewModel updateBlogTagViewModel)
        {
            try
            {
                var result = await _blogTagService.UpdateBlogTag(updateBlogTagViewModel);
                if (result)
                {
                    return Ok("Liên kết đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Liên kết đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteBlogTag(Guid id)
        {
            try
            {
                var result = await _blogTagService.DeleteBlogTag(id);
                if (result)
                {
                    return Ok("Liên kết đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Liên kết đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
