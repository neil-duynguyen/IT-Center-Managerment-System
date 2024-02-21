using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{

    public class DivisionRepository : GenericRepository<Division>, IDivisionRepository
    {
        private readonly AppDbContext _dbContext;
        public DivisionRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Division>> GetDivisionByName(string name)
        {
            var divisions = await _dbContext.Division
                .Where(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return divisions;
        }
    }
}
