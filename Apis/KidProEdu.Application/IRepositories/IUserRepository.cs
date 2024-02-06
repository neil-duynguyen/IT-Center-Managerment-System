using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByUserNameAndPasswordHash(string username, string passwordHash);

        Task<bool> CheckUserNameExited(string username);
    }
}
