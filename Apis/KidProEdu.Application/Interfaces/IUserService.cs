using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

namespace KidProEdu.Application.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateAccountAsync(CreateUserViewModel userObject);
        public Task<LoginViewModel> LoginAsync(UserLoginViewModel userObject);
        public Task<UserViewModel> GetUserById(Guid id);
        public Task<List<UserViewModel>> GetAllUser();
        public Task<List<UserViewModel>> GetUserByRoleId(Guid Id);
        public Task<UserViewModel> ChangePassword(ChangePasswordViewModel changePasswordViewModel);
        public Task<bool> UpdateUser(UpdateUserViewModel updateUserViewModel, params Expression<Func<UserAccount, object>>[] uniqueProperties);
        public Task<bool> DeleteUser(Guid id);
        public Task<bool> ChangeStatusUser(Guid[] listId);
    }
}
