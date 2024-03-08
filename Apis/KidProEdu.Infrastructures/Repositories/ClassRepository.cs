using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KidProEdu.Infrastructures.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        private readonly AppDbContext _dbContext;
        public ClassRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Class>> GetClassByClassCode(string classCode)
        {
            var requests = await _dbContext.Class
                .Where(x => x.ClassCode.ToLower() == classCode.ToLower() && x.IsDeleted == false 
                && (x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending) 
                || x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Started)))
                .ToListAsync();

            return requests;
        }

        public override async Task<Class> GetByIdAsync(Guid id)
        { 
            return await _dbContext.Class.Include(x => x.Course).Include(x => x.Enrollments).Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
