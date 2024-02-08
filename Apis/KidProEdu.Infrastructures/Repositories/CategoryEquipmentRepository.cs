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
    public class CategoryEquipmentRepository : GenericRepository<CategoryEquipment>, ICategoryEquipmentRepository
    {
        private readonly AppDbContext _dbContext;
        public CategoryEquipmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<CategoryEquipment>> GetCategoryEquipmentByName(string name)
        {
            var categoryEquipments = await _dbContext.CategoryEquipment
                .Where(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return categoryEquipments;
        }

     


    }
}