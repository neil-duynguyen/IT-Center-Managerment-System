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

        public async Task<List<Request>> GetRequestByRequestType(string requestType)
        {
            var requests = await _dbContext.Request
                .Where(x => x.RequestType.ToLower() == requestType.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return requests;
        }
    }
}
