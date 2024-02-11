using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;


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

        [HttpPut()]
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



    }
}