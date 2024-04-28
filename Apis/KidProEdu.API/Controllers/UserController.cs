using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;


namespace KidProEdu.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginViewModel loginObject)
        {
            try
            {
                var result = await _userService.LoginAsync(loginObject);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }

        [HttpPost]
        [Route("Register")]
        [Authorize(Roles = ("Admin, Manager, Staff"))]
        public async Task<IActionResult> CreateAccountAsync(CreateUserViewModel loginObject)
        {
            try
            {               
                return Ok(await _userService.CreateAccountAsync(loginObject));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UserAsync(Guid id)
        {
            try
            {
                return Ok(await _userService.GetUserById(id));
            }
            catch (Exception)
            {
                return NotFound("Not Found");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> UserAsync()
        {
            try
            {
                return Ok(await _userService.GetAllUser());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet()]
        [Route("UserByRoleId/{id}")]
        public async Task<IActionResult> UserByRoleIdAsync(Guid id)
        {
            try
            {
                return Ok(await _userService.GetUserByRoleId(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                return Ok(await _userService.ChangePassword(changePasswordViewModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUser(UpdateUserViewModel updateUserViewModel)
        {
            try
            {
                var result = await _userService.UpdateUser(updateUserViewModel, x => x.UserName, x => x.Email, x => x.Phone);
                if (result)
                {
                    return Ok("User đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("User đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                if (result)
                {
                    return Ok("User đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("User đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("ChangeStatus")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> ChangeStatusUser(Guid[] ids)
        {
            try
            {
                var result = await _userService.ChangeStatusUser(ids);
                if (result)
                {
                    return Ok("User cập nhật trạng thái thành công.");
                }
                else
                {
                    return BadRequest("User cập nhật trạng thái thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [Route("GetTeacherFree")]
        public async Task<IActionResult> GetTeacherFree()
        {
            try
            {
                return Ok(await _userService.GetTeacherFree());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}