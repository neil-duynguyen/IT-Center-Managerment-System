using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KidProEdu.Infrastructures.Repositories
{
    public class AdviseRequestRepository : GenericRepository<AdviseRequest>, IAdviseRequestRepository
    {
        private readonly AppDbContext _dbContext;
        public AdviseRequestRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public override async Task<List<AdviseRequest>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.UserAccount).Include(x => x.Location).Include(x => x.Slot)
                .Where(x => x.IsTested == false).ToListAsync();
        }
        public async Task<AdviseRequest> GetAdviseRequestByEmail(string email)
        {
            var adviseRequests = await _dbContext.AdviseRequest
                .Where(x => x.Email.ToLower() == email.ToLower() && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return adviseRequests;
        }

        public async Task<AdviseRequest> GetAdviseRequestByPhone(string phone)
        {
            var adviseRequests = await _dbContext.AdviseRequest
                .Where(x => x.Phone.ToLower() == phone.ToLower() && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return adviseRequests;
        }

        public async Task<List<AdviseRequest>> GetAdviseRequestByTestDate(DateTime testDate)
        {
            var adviseRequests = await _dbContext.AdviseRequest
                .Where(x => x.TestDate.Date == testDate.Date && x.IsDeleted == false)
                .OrderByDescending(x => x.TestDate).ToListAsync();

            return adviseRequests;
        }

        public async Task<AdviseRequest> GetAdviseRequestByProperty(UpdateAdviseRequestViewModel updateAdviseRequestViewModel, Expression<Func<AdviseRequest, object>> property)
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
                case nameof(AdviseRequest.Email):
                    return await GetAdviseRequestByEmail(updateAdviseRequestViewModel.Email);
                case nameof(AdviseRequest.Phone):
                    return await GetAdviseRequestByPhone(updateAdviseRequestViewModel.Phone);
                // Thêm các trường hợp xử lý khác nếu cần
                default:
                    throw new ArgumentException($"Property {propertyName} is not supported.");
            }
        }

        public async Task<List<AdviseRequest>> GetAdviseRequestByUserId(Guid id)
        {
            var adviseRequest = await _dbContext.AdviseRequest.Where(x => x.UserId == id && x.IsDeleted == false)
                .OrderByDescending(x => x.CreationDate).ToListAsync();

            return adviseRequest;
        }
    }
}
