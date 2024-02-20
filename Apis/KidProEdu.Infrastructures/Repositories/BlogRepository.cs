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

        public override async Task<List<Blog>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Tags).Include(x => x.UserAccount).Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<Blog> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Tags).Include(x => x.UserAccount).Where(x => !x.IsDeleted).FirstAsync(x => x.Id == id);
        }
    }
}

