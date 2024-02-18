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
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{

    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {

        private readonly AppDbContext _dbContext;
        public BlogRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<Blog> GetBlogWithUserByBlogId(Guid id)
        {


            var blog = await _dbContext.Blogs
                .Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.UserAccount)
                .Include(x => x.BlogTags)
                .FirstAsync();
            return blog;

        }
    }
}

