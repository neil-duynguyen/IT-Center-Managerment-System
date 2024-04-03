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
    public class ChildrenRepository : GenericRepository<ChildrenProfile>, IChildrenRepository
    {
        private readonly AppDbContext _dbContext;
        public ChildrenRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public override async Task<ChildrenProfile> GetByIdAsync(Guid id)
        {
            return await _dbContext.ChildrenProfile
                .Include(x=>x.UserAccount)
                .Include(x => x.Enrollments).ThenInclude(x => x.Class).ThenInclude(x => x.Course)
                .Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
        }
        public override async Task<List<ChildrenProfile>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.ChildrenAnswers).Include(x => x.Enrollments).ThenInclude(x => x.Class).ThenInclude(x => x.Course).
                Where(x => !x.IsDeleted).ToListAsync();
        }
    }
}
