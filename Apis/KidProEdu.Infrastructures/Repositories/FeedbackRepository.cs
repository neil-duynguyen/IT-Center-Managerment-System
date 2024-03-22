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
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly AppDbContext _dbContext;
        public FeedbackRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Feedback>> GetFeedbackByClassId(Guid id)
        {
            var feedbacks = await _dbContext.Feedback
                .Where(x => x.ClassId == id && x.IsDeleted == false)
                .ToListAsync();

            return feedbacks;
        }

        public async Task<List<Feedback>> GetFeedbackByUserId(Guid id)
        {
            var feedbacks = await _dbContext.Feedback
                .Where(x => x.UserId == id && x.IsDeleted == false)
                .ToListAsync();

            return feedbacks;
        }

    }
}
