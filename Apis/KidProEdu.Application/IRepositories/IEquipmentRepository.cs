using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface IEquipmentRepository : IGenericRepository<Equipment>
    {
        Task<List<Equipment>> GetListEquipmentByName(string name);
        Task<List<Equipment>> GetListEquipmentByCateId(Guid cateId);
        Task<List<Equipment>> GetListEquipmentByCateIdAndStatus(Guid cateId, StatusOfEquipment status);
        Task<List<Equipment>> GetListEquipmentByStatus(StatusOfEquipment status);
    }
}
