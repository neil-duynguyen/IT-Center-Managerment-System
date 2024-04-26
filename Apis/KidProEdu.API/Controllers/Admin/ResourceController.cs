using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.ResourceViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet("Resources")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Resources()
        {
            return Ok(await _resourceService.GetResources());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Resource(Guid id)
        {
            var result = await _resourceService.GetResourceById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("ResourceByCourseId/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ResourceByCourseId(Guid id)
        {
            var result = await _resourceService.GetResourcesByCourseId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> PostResource(CreateResourceViewModel createResourceViewModel)
        {
            try
            {
                var result = await _resourceService.CreateResource(createResourceViewModel);
                if (result)
                {
                    return Ok("Nguồn tài liệu đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Nguồn tài liệu đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> PutResource(UpdateResourceViewModel updateResourceViewModel)
        {
            try
            {
                var result = await _resourceService.UpdateResource(updateResourceViewModel);
                if (result)
                {
                    return Ok("Nguồn tài liệu đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Nguồn tài liệu đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> DeleteResource(Guid id)
        {
            try
            {
                var result = await _resourceService.DeleteResource(id);
                if (result)
                {
                    return Ok("Nguồn tài liệu đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Nguồn tài liệu đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
