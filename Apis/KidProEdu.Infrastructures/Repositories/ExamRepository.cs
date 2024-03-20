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

    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        private readonly AppDbContext _dbContext;
        public ExamRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Exam>> GetExamByCourseId(Guid id)
        {
            var exams = await _dbContext.Exam.Where(x => x.CourseId == id && x.IsDeleted == false).ToListAsync();
            return exams;
        }

        public async Task<Exam> GetExamByTestName(string name)
        {
            var exam = await _dbContext.Exam.FirstOrDefaultAsync(x => x.TestName.ToLower() == name.ToLower() && x.IsDeleted == false);
            return exam;
        }
    }
}
