using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        private readonly AppDbContext _dbContext;

        public SemesterRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<Semester> GetSemesterBySemesterName(string semesterName)
        {
            var semesters = await _dbContext.Semester
                .Where(x => x.SemesterName.ToLower() == semesterName.ToLower() && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return semesters;
        }
        
        public async Task<Semester> GetSemesterByStartDate(DateTime startDate)
        {
            var semesters = await _dbContext.Semester
                .Where(x => x.StartDate.Date == startDate.Date && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return semesters;
        }

        public async Task<Semester> GetSemesterByProperty(UpdateSemesterViewModel updateSemesterViewModel, Expression<Func<Semester, object>> property)
        {
            var body = property.Body;
            MemberExpression memberExpression;

            // Nếu biểu thức lambda là một UnaryExpression, ta cần lấy Operand
            if (body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            // Nếu biểu thức lambda là một MemberExpression
            else if (body is MemberExpression)
            {
                memberExpression = (MemberExpression)body;
            }
            else
            {
                throw new ArgumentException("Invalid expression type");
            }

            var propertyName = memberExpression.Member.Name;

            switch (propertyName)
            {
                case nameof(Semester.SemesterName):
                    return await GetSemesterBySemesterName(updateSemesterViewModel.SemesterName);
                case nameof(Semester.StartDate):
                    return await GetSemesterByStartDate(updateSemesterViewModel.StartDate);
                // Thêm các trường hợp xử lý khác nếu cần
                default:
                    throw new ArgumentException($"Property {propertyName} is not supported.");
            }
        }
    }
}
