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

    public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
    {
        private readonly AppDbContext _dbContext;
        public EquipmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Equipment>> GetEquipmentByCode(string code)
        {
            var equipments = await _dbContext.Equipment
                .Where(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return equipments;
        }

        public async Task<List<Equipment>> GetEquipmentByName(string name)
        {
            var equipments = await _dbContext.Equipment
                .Where(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == false)
                .ToListAsync();

            return equipments;
        }
    }
}
