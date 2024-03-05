using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.DivisionUserAccountViewModels;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionUserAccountController : ControllerBase
    {
        private readonly IDivisionUserAccountService _divisionUserAccountService;
        public DivisionUserAccountController(IDivisionUserAccountService divisionUserAccountService)
        {
            _divisionUserAccountService = divisionUserAccountService;
        }

        [HttpGet("DivisionUserAccounts")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DivisionUserAccounts()
        {
            return Ok(await _divisionUserAccountService.GetDivisionUserAccounts());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DivisionUserAccount(Guid id)
        {
            var DivisionUserAccount = await _divisionUserAccountService.GetDivisionUserAccountById(id);
            if (DivisionUserAccount == null)
            {
                return NotFound();
            }
            return Ok(DivisionUserAccount);
        }

        [HttpGet("DivisionUserAccountByUserId/{userId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DivisionUserAccountByUserId(Guid userId)
        {
            var DivisionUserAccount = await _divisionUserAccountService.GetDivisionUserAccountByUserId(userId);
            if (DivisionUserAccount == null)
            {
                return NotFound();
            }
            return Ok(DivisionUserAccount);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostDivisionUserAccount(CreateDivisionUserAccountViewModel createDivisionUserAccountViewModel)
        {
            try
            {
                var result = await _divisionUserAccountService.CreateDivisionUserAccount(createDivisionUserAccountViewModel);
                if (result)
                {
                    return Ok("Liên kết người dùng và phòng ban đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Liên kết người dùng và phòng ban đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutDivisionUserAccount(UpdateDivisionUserAccountViewModel updateDivisionUserAccountViewModel)
        {
            try
            {
                var result = await _divisionUserAccountService.UpdateDivisionUserAccount(updateDivisionUserAccountViewModel);
                if (result)
                {
                    return Ok("Liên kết người dùng và phòng ban đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Liên kết người dùng và phòng ban đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteDivisionUserAccount(Guid id)
        {
            try
            {
                var result = await _divisionUserAccountService.DeleteDivisionUserAccount(id);
                if (result)
                {
                    return Ok("Liên kết người dùng và phòng ban đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Liên kết người dùng và phòng ban đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
