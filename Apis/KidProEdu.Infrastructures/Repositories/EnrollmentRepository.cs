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
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly AppDbContext _dbContext;
        public EnrollmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public override async Task<List<Enrollment>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Class).Include(x => x.ChildrenProfile).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByClassId(Guid Id)
        {
            return await _dbContext.Enrollment
                .Include(x => x.Class)
                .Include(x => x.ChildrenProfile)
                .Where(x => x.ClassId == Id && !x.IsDeleted).ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentsByClassIdAndChildrenProfileId(Guid classId, Guid childId)
        {
            return _dbContext.Enrollment.FirstOrDefault(x => x.ClassId == classId && x.ChildrenProfileId == childId && !x.IsDeleted);
        }
    }
}
