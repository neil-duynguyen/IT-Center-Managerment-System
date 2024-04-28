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
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        private readonly AppDbContext _dbContext;
        public ContractRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Contract>> GetContractByCode(string code)
        {
            var contracts = await _dbContext.Contract
                .Where(x => x.ContractCode.ToLower() == code.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return contracts;
        }

        public async Task<List<Contract>> GetContractByTeacherId(Guid teacherId)
        {
            var contracts = await _dbContext.Contract
                .Where(x => x.UserId == teacherId && x.IsDeleted == false)
                .ToListAsync();

            return contracts;
        }
    }
}
