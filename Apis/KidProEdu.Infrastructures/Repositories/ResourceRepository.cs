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

    public class ResourceRepository : GenericRepository<Resource>, IResourceRepository
    {
        private readonly AppDbContext _dbContext;
        public ResourceRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Resource>> GetResourcesByCourseId(Guid id)
        {
            return await _dbContext.Resource.Where(x => x.CourseId == id && !x.IsDeleted).ToListAsync();
        }
    }
}
