using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using KidProEdu.Infrastructures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructures.Repositories
{
    public class UserRepository : GenericRepository<UserAccount>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckUserNameExited(CreateUserViewModel userObject)
        {
            if (await _dbContext.UserAccount.FirstOrDefaultAsync(u => u.UserName.ToLower() == userObject.UserName) != null)
                throw new Exception("UserName đã tồn tại");

            if (await _dbContext.UserAccount.FirstOrDefaultAsync(u => u.Email.ToLower() == userObject.Email) != null)
                throw new Exception("Email đã tồn tại");

            if (await _dbContext.UserAccount.FirstOrDefaultAsync(u => u.Phone.ToLower() == userObject.Phone) != null)
                throw new Exception("Phone đã tồn tại");
            return true;
        }

        public async Task<UserAccount> GetUserByUserNameAndPasswordHash(string username, string passwordHash)
        {
            var user = await _dbContext.UserAccount.Include(u => u.Role)
                .FirstOrDefaultAsync(record => record.UserName == username
                                        && record.PasswordHash == passwordHash);
            if (user is null)
            {
                throw new Exception("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return user;
        }

        public override async Task<List<UserAccount>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Role).Include(x => x.Location).Include(x => x.ChildrenProfile).Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<UserAccount> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Role).Include(x => x.ChildrenProfile).Include(x => x.Location).Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserAccount> GetUserAccountByProperty(UpdateUserViewModel updateUserViewModel, Expression<Func<UserAccount, object>> property)
        {
            var body = property.Body;
            MemberExpression memberExpression;

            // Nếu biểu thức lambda là một UnaryExpression, ta cần lấy Operand
            if (body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            // Nếu biểu thức lambda là một MemberExpression
            else if (body is MemberExpression)
            {
                memberExpression = (MemberExpression)body;
            }
            else
            {
                throw new ArgumentException("Invalid expression type");
            }

            var propertyName = memberExpression.Member.Name;

            switch (propertyName)
            {
                case nameof(UserAccount.UserName):
                    return await _dbContext.UserAccount
                        .Where(x => x.UserName.ToLower().Equals(updateUserViewModel.UserName.ToLower()) && x.IsDeleted == false)
                        .FirstOrDefaultAsync();
                case nameof(UserAccount.Email):
                    return await _dbContext.UserAccount
                        .Where(x => x.Email.ToLower().Equals(updateUserViewModel.Email.ToLower()) && x.IsDeleted == false)
                        .FirstOrDefaultAsync();
                case nameof(UserAccount.Phone):
                    return await _dbContext.UserAccount
                        .Where(x => x.Phone.ToLower().Equals(updateUserViewModel.Phone.ToLower()) && x.IsDeleted == false)
                        .FirstOrDefaultAsync();
                // Thêm các trường hợp xử lý khác nếu cần
                default:
                    throw new ArgumentException($"Property {propertyName} is not supported.");
            }
        }

        public async Task<List<UserAccount>> GetTeacherByJobType(JobType jobType)
        {
            var teachers = await _dbContext.UserAccount.Include(x => x.Role)
                .Include(x => x.Contracts).ThenInclude(x => x.ConfigJobType)
                .Where(x => x.Role.Name.ToLower() == "Teacher" && x.IsDeleted == false
                && x.Status.Equals(KidProEdu.Domain.Enums.StatusUser.Enable)
                && x.Contracts.OrderByDescending(x => x.CreationDate).FirstOrDefault().ConfigJobType.JobType.Equals(jobType)
                )
                .ToListAsync();

            return teachers;
        }

        public async Task<int> GetTotalParents(DateTime startDate, DateTime endDate)
        {
            var totalParents = await _dbContext.UserAccount
                .Include(x => x.Role)
                .Where(x => x.Role.Name == "Parent" && x.Status == StatusUser.Enable && x.CreationDate >= startDate && x.CreationDate <= endDate && !x.IsDeleted)
                .AsNoTracking()
                .CountAsync();
            return totalParents == 0 ? 0 : totalParents;
        }

        public async Task<int> GetTotalStaffs(DateTime startDate, DateTime endDate)
        {
            var totalStaffs = await _dbContext.UserAccount
                .Include(x => x.Role)
                .Where(x => x.Role.Name == "Staff" && x.Status == StatusUser.Enable && x.CreationDate >= startDate && x.CreationDate <= endDate && !x.IsDeleted)
                .AsNoTracking()
                .CountAsync();
            return totalStaffs == 0 ? 0 : totalStaffs;
        }

        public async Task<int> GetTotalManagers(DateTime startDate, DateTime endDate)
        {
            var totalManagers = await _dbContext.UserAccount
                .Include(x => x.Role)
                .Where(x => x.Role.Name == "Manager" && x.Status == StatusUser.Enable && x.CreationDate >= startDate && x.CreationDate <= endDate && !x.IsDeleted)
                .AsNoTracking()
                .CountAsync();
            return totalManagers == 0 ? 0 : totalManagers;
        }

        public async Task<int> GetTotalTeachers(DateTime startDate, DateTime endDate)
        {
            var totalTeachers = await _dbContext.UserAccount
                .Include(x => x.Role)
                .Where(x => x.Role.Name == "Teacher" && x.Status == StatusUser.Enable && x.CreationDate >= startDate && x.CreationDate <= endDate && !x.IsDeleted)
                .AsNoTracking()
                .CountAsync();
            return totalTeachers == 0 ? 0 : totalTeachers;
        }
    }
}
