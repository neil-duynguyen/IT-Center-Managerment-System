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
    public class ChildrenAnswerRepository : GenericRepository<ChildrenAnswer>, IChildrenAnswerRepository
    {
        private readonly AppDbContext _dbContext;
        public ChildrenAnswerRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<ChildrenAnswer> GetChildrenAnswerWithChildrenProfileIdExamIdQuestionId(Guid childId, Guid examId, Guid questionId)
        {
            var childrenAnswer = await _dbContext.ChildrenAnswer.FirstOrDefaultAsync(x => x.ChildrenProfileId == childId && x.ExamId == examId && x.QuestionId == questionId && !x.IsDeleted);
            return childrenAnswer;
        }

        public override async Task<List<ChildrenAnswer>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Exam).Include(x => x.ChildrenProfile).Include(x => x.Question).Where(x => !x.IsDeleted).ToListAsync();
        }
    }
}
