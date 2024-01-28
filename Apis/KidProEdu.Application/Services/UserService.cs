using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace KidProEdu.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
        }

        public async Task<LoginViewModel> LoginAsync(UserLoginDTO userObject)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUserNameAndPasswordHash(userObject.UserName, userObject.Password.Hash());
            if (user == null)
            {
                throw new Exception("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            var token = user.GenerateJsonWebToken(_configuration["AppSettings:SecretKey"]);

            return new LoginViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Token = token,
                Role = user.Role.Name
            };
        }

        public async Task CreateAccountAsync(CreateAccount userObject)
        {
            // check username exited
            var isExited = await _unitOfWork.UserRepository.CheckUserNameExited(userObject.UserName);

            if (isExited)
            {
                throw new Exception("Username exited please try again");
            }

            var newUser = new User
            {
                RoleId = userObject.RoleId,
                UserName = userObject.UserName,
                PasswordHash = new String("123").Hash(),
                FullName = userObject.FullName,
                GenderType = userObject.GenderType,
                Email = userObject.Email,
                Phone = userObject.Phone,
                Address = userObject.Address,
                CreationDate = _currentTime.GetCurrentTime(),
                IsDeleted = false
            };

            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
