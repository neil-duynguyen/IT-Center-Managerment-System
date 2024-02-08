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
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        private readonly AppDbContext _dbContext;

        public SemesterRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Semester>> GetSemesterBySemesterName(string semesterName)
        {
            var semesters = await _dbContext.Semesters
                .Where(x => x.SemesterName.ToLower() == semesterName.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return semesters;
        }
        
        public async Task<List<Semester>> GetSemesterByStartDate(DateTime startDate)
        {
            var semesters = await _dbContext.Semesters
                .Where(x => x.StartDate.Date == startDate.Date && x.IsDeleted == false)
                .ToListAsync();

            return semesters;
        }
    }
}
