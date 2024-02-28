using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.ViewModels.ClassViewModels;
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
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        private readonly AppDbContext _dbContext;
        public ClassRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Class>> GetClassByClassCode(string classCode)
        {
            var requests = await _dbContext.Class
                .Where(x => x.ClassCode.ToLower() == classCode.ToLower() && x.IsDeleted == false 
                && (x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Pending) 
                || x.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Started)))
                .ToListAsync();

            return requests;
        }
    }
}
