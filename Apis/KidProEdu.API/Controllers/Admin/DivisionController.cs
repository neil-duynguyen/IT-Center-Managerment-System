using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private readonly IDivisionService _divisionService;
        public DivisionController(IDivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        [HttpGet("Divisions")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Divisions()
        {
            return Ok(await _divisionService.GetDivisions());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Division(Guid id)
        {
            var Division = await _divisionService.GetDivisionById(id);
            if (Division == null)
            {
                return NotFound();
            }
            return Ok(Division);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostDivision(CreateDivisionViewModel createDivisionViewModel)
        {
            try
            {
                var result = await _divisionService.CreateDivision(createDivisionViewModel);
                if (result)
                {
                    return Ok("Phòng ban đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Phòng ban đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutDivision(UpdateDivisionViewModel updateDivisionViewModel)
        {
            try
            {
                var result = await _divisionService.UpdateDivision(updateDivisionViewModel);
                if (result)
                {
                    return Ok("Phòng ban đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Phòng ban đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteDivision(Guid id)
        {
            try
            {
                var result = await _divisionService.DeleteDivision(id);
                if (result)
                {
                    return Ok("Phòng ban đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Phòng ban đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
