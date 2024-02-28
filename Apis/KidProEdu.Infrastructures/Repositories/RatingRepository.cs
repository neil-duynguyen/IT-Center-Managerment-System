using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{

    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly AppDbContext _dbContext;
        public RatingRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Rating>> GetListRatingByCourseId(Guid CourseId)
        {
            return await _dbContext.Rating.Include(x => x.Course).Where(x => !x.IsDeleted && x.CourseId == CourseId).ToListAsync();
        }

        public override async Task<List<Rating>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Course).Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<Rating> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Course).Where(x => !x.IsDeleted).FirstAsync(x => x.Id == id);
        }

    }
}
