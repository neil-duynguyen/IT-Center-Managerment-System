using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<UserAccount>
    {
        Task<UserAccount> GetUserByUserNameAndPasswordHash(string username, string passwordHash);

        Task<bool> CheckUserNameExited(string username);
    }
}
