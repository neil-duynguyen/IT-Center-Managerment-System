using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;

namespace KidProEdu.Application.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateAccountAsync(CreateUserViewModel userObject);
        public Task<LoginViewModel> LoginAsync(UserLoginViewModel userObject);
    }
}
