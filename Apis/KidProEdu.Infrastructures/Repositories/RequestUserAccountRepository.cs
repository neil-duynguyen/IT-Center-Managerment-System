using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{
    public class RequestUserAccountRepository : GenericRepository<RequestUserAccount>, IRequestUserAccountRepository
    {

        private readonly AppDbContext _dbContext;
        public RequestUserAccountRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<RequestUserAccount>> GetRequestUserByRequestId(Guid requestId)
        {
            var requestUser = await _dbContext.RequestUserAccount.Include(x => x.UserAccount).Include(x => x.Request)
                .Where(x => x.IsDeleted == false && x.RequestId == requestId).ToListAsync();
            return requestUser;
        }

        public async Task<List<RequestUserAccount>> GetRequestUserByRecieverId(Guid recieverId)
        {
            var requestUser = await _dbContext.RequestUserAccount.Include(x => x.UserAccount).Include(x => x.Request)
                .Where(x => x.IsDeleted == false && x.RecieverId == recieverId && x.Request.IsDeleted == false).ToListAsync();
            return requestUser;
        }
    }
}
