using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

namespace KidProEdu.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<UserAccount>
    {
        Task<UserAccount> GetUserByUserNameAndPasswordHash(string username, string passwordHash);

        Task<bool> CheckUserNameExited(CreateUserViewModel username);

        Task<UserAccount> GetUserAccountByProperty(UpdateUserViewModel updateUserViewModel, Expression<Func<UserAccount, object>> property);

    }
}
