using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{

    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly AppDbContext _dbContext;
        public TagRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Tag>> GetTagByTagName(string tagName)
        {
            var tags = await _dbContext.Tag
                .Where(x => x.TagName.ToLower() == tagName.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return tags;
        }
    }
}
