using DocumentFormat.OpenXml.Wordprocessing;
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

        public async Task<CategoryEquipment> GetCategoryEquipmentByEquipmentId(Guid equipmentId)
        {
            var categoryEquipment = await _dbContext.CategoryEquipment
                .Include(ce => ce.Equipments) 
                .FirstOrDefaultAsync(ce => ce.Equipments.Any(e => e.Id == equipmentId && e.IsDeleted == false) && ce.IsDeleted == false);
            return categoryEquipment;
        }

        public async Task<CategoryEquipment> GetCategoryEquipmentByName(string name)
        {
            var categoryEquipment = await _dbContext.CategoryEquipment.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == false);
            return categoryEquipment;
        }

        public override async Task<List<CategoryEquipment>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Equipments).ThenInclude(x => x.LogEquipments).Where(x => !x.IsDeleted).ToListAsync();
        }


    }
}