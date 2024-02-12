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

    public class BlogTagRepository : GenericRepository<BlogTag>, IBlogTagRepository
    {
        private readonly AppDbContext _dbContext;
        public BlogTagRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<BlogTag>> GetBlogTagByBlogId(Guid id)
        {
            var blogTags = await _dbContext.BlogsTag
                .Where(x => x.BlogId == id && x.IsDeleted == false)
                .ToListAsync();

            return blogTags;
        }
    }
}
