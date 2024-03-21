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

        public async Task<List<Class>> GetClassBySlot(int slot)
        {
            var classes = new List<Class>();
            switch (slot)
            {
                case 1:
                    classes = await _dbContext.Class.Include(x => x.Schedules).ThenInclude(x => x.Slot)
                    .Where(x => x.Schedules.FirstOrDefault().Slot.Name.ToLower() == "Slot1".ToLower() && x.IsDeleted == false
                    && x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending))
                    .ToListAsync();
                    break;
                case 2:
                    classes = await _dbContext.Class.Include(x => x.Schedules).ThenInclude(x => x.Slot)
                    .Where(x => x.Schedules.FirstOrDefault().Slot.Name.ToLower() == "Slot2".ToLower() && x.IsDeleted == false
                    && x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending))
                    .ToListAsync();
                    break;
                case 3:
                    classes = await _dbContext.Class.Include(x => x.Schedules).ThenInclude(x => x.Slot)
                    .Where(x => x.Schedules.FirstOrDefault().Slot.Name.ToLower() == "Slot3".ToLower() && x.IsDeleted == false
                    && x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending))
                    .ToListAsync();
                    break;
                case 4:
                    classes = await _dbContext.Class.Include(x => x.Schedules).ThenInclude(x => x.Slot)
                    .Where(x => x.Schedules.FirstOrDefault().Slot.Name.ToLower() == "Slot4".ToLower() && x.IsDeleted == false
                    && x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending))
                    .ToListAsync();
                    break;
                case 5:
                    classes = await _dbContext.Class.Include(x => x.Schedules).ThenInclude(x => x.Slot)
                    .Where(x => x.Schedules.FirstOrDefault().Slot.Name.ToLower() == "Slot5".ToLower() && x.IsDeleted == false
                    && x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending))
                    .ToListAsync();
                    break;
                /*case 6:
                    classes = await _dbContext.Class.Include(x => x.Schedules).ThenInclude(x => x.Slot)
                    .Where(x => x.Schedules.FirstOrDefault().Slot.Name.ToLower() == "Slot6".ToLower() && x.IsDeleted == false
                    && x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending))
                    .ToListAsync();
                    break;*/
                default:
                    break;
            }

            return classes;
        }

        public override async Task<Class> GetByIdAsync(Guid id)
        {
            return await _dbContext.Class.Include(x => x.Course).Include(x => x.Enrollments).Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
