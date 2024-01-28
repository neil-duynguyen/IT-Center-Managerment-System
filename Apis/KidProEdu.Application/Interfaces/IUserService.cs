using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;

namespace KidProEdu.Application.Interfaces
{
    public interface IUserService
    {
        public Task CreateAccountAsync(CreateAccount userObject);
        public Task<LoginViewModel> LoginAsync(UserLoginDTO userObject);
    }
}
