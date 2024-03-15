using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.SkillViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("Skills")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Skills()
        {
            return Ok(await _skillService.GetSkills());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Skill(Guid id)
        {
            var skill = await _skillService.GetSkillById(id);
            if (skill == null)
            {
                return NotFound();
            }
            return Ok(skill);
        }

        [HttpGet("SkillByName/{name}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> SkillByName(string name)
        {
            var skill = await _skillService.GetSkillByName(name);
            if (skill == null)
            {
                return NotFound();
            }
            return Ok(skill);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostSkill(CreateSkillViewModel createSkillViewModel)
        {
            try
            {
                var result = await _skillService.CreateSkill(createSkillViewModel);
                if (result)
                {
                    return Ok("Kĩ năng đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Kĩ năng đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutSkill(UpdateSkillViewModel updateSkillViewModel)
        {
            try
            {
                var result = await _skillService.UpdateSkill(updateSkillViewModel);
                if (result)
                {
                    return Ok("Kĩ năng đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Kĩ năng đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
            try
            {
                var result = await _skillService.DeleteSkill(id);
                if (result)
                {
                    return Ok("Kĩ năng đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Kĩ năng đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
