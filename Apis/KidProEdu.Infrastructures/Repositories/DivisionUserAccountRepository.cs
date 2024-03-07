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
    public class DivisionUserAccountRepository : GenericRepository<DivisionUserAccount>, IDivisionUserAccountRepository
    {
        private readonly AppDbContext _dbContext;
        public DivisionUserAccountRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<DivisionUserAccount> GetDivisionUserAccountByUserId(Guid id)
        {
            var divisionUserAccount = await _dbContext.DivisionUserAccount
                .FirstOrDefaultAsync(x => x.UserAccountId == id && !x.IsDeleted);

            return divisionUserAccount;
        }
    }
}
