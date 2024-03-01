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
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _dbContext;
        public QuestionRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Question>> GetQuestionByTitle(string title)
        {
            var questions = await _dbContext.Question
                .Where(x => x.Title.ToLower() == title.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return questions;
        }

        public async Task<List<Question>> GetQuestionByLesson(Guid id)
        {
            var questions = await _dbContext.Question
                .Where(x => x.Id == id && x.IsDeleted == false)
                .ToListAsync();

            return questions;
        }
    }
}
