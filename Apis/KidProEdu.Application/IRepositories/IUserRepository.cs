using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System.Linq.Expressions;

namespace KidProEdu.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<UserAccount>
    {
        Task<UserAccount> GetUserByUserNameAndPasswordHash(string username, string passwordHash);

        Task<bool> CheckUserNameExited(CreateUserViewModel username);

        Task<UserAccount> GetUserAccountByProperty(UpdateUserViewModel updateUserViewModel, Expression<Func<UserAccount, object>> property);

        Task<List<UserAccount>> GetTeacherByJobType(JobType jobType);

        Task<int> GetTotalParents(DateTime startDate, DateTime endDate);
        Task<int> GetTotalStaffs(DateTime startDate, DateTime endDate);
        Task<int> GetTotalManagers(DateTime startDate, DateTime endDate);
        Task<int> GetTotalTeachers(DateTime startDate, DateTime endDate);

    }
}
