using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
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
            return await _dbSet.AsNoTracking().Include(x => x.Class).Include(x => x.ChildrenProfile).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<double> GetCommissionEnrollmentsTotalAmount(DateTime startDate, DateTime endDate)
        {
            var totalAmount = await _dbContext.Enrollment
                .Where(x => x.CreationDate >= startDate && x.CreationDate <= endDate && !x.IsDeleted)
                .AsNoTracking()
                .SumAsync(t => t.Commission ?? 0);
            return totalAmount;
        }

        public async Task<List<Enrollment>> GetEnrollmentsByChildId(Guid Id)
        {
            return await _dbContext.Enrollment
               .AsNoTracking()
               .Where(x => x.ChildrenProfileId == Id && !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByClassId(Guid Id)
        {
            return await _dbContext.Enrollment
                .AsNoTracking()
                .Include(x => x.Class)
                .Include(x => x.ChildrenProfile).ThenInclude(x => x.UserAccount)
                .Where(x => x.ClassId == Id && !x.IsDeleted).ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentsByClassIdAndChildrenProfileId(Guid classId, Guid childId)
        {
            return _dbContext.Enrollment
                .AsNoTracking()
                .FirstOrDefault(x => x.ClassId == classId && x.ChildrenProfileId == childId && !x.IsDeleted);
        }
    }
}
