using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KidProEdu.Infrastructures.Repositories
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        private readonly AppDbContext _dbContext;
        public RequestRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public override async Task<List<Request>> GetAllAsync()
        {
            var requests = await _dbContext.Request.Include(x => x.RequestUserAccounts)
                .Where(x => !x.IsDeleted).ToListAsync();
            return requests;
        }

        public async Task<List<Request>> GetRequestByUser(Guid id)
        {
            var requests = await _dbContext.Request.Include(x => x.RequestUserAccounts)
                .Where(x => x.CreatedBy == id && x.IsDeleted == false)
                .ToListAsync();

            return requests;
        }
    }
}
