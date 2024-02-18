using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("Blogs")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Blogs()
        {
            return Ok(await _blogService.GetBlogs());
        }

        [HttpGet("BlogWithUser/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> BlogWithUser(Guid id)
        {
            var blog = await _blogService.GetBlogWithUserByBlogId(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Blog(Guid id)
        {
            var blog = await _blogService.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostBlog(CreateBlogViewModel createBlogViewModel)
        {
            try
            {
                var result = await _blogService.CreateBlog(createBlogViewModel);
                if (result)
                {
                    return Ok("Bài viết đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Bài viết đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutBlog(UpdateBlogViewModel updateBlogViewModel)
        {
            try
            {
                var result = await _blogService.UpdateBlog(updateBlogViewModel);
                if (result)
                {
                    return Ok("Bài viết đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Bài viết đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            try
            {
                var result = await _blogService.DeleteBlog(id);
                if (result)
                {
                    return Ok("Bài viết đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Bài viết đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
