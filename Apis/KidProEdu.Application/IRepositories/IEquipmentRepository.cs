using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface IEquipmentRepository : IGenericRepository<Equipment>
    {
        Task<List<Equipment>> GetEquipmentByName(string name);
        Task<List<Equipment>> GetEquipmentByCode(string code);
    }
}
