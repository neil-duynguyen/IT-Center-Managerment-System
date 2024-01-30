using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;


namespace KidProEdu.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
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










    }
}