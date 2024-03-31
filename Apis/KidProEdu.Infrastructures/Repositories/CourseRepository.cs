﻿using Infrastructures.Repositories;
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
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly AppDbContext _dbContext;
        public CourseRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public Task<bool> CheckNameExited(string name) => _dbContext.Course.AnyAsync(u => u.Name == name);

        public async Task<List<Course>> GetListCourseByChildrenId(Guid childrenId)
        {
            var courses = await _dbContext.Course
            .Where(x => x.Classes
            .Any(c => c.Course.Classes
            .Any(cp => cp.Enrollments
            .Any(ci => ci.ChildrenProfileId == childrenId && !ci.IsDeleted))))
            .ToListAsync();
            return courses;
        }
    }
}
