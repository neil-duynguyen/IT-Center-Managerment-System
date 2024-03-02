using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface ILogEquipmentRepository : IGenericRepository<LogEquipment>
    {
        Task<List<LogEquipment>> GetLogEquipmentByUserId(Guid userId);
        Task<List<LogEquipment>> GetLogEquipmentByEquipmentId(Guid equipmentId);
        Task<List<LogEquipment>> GetLogEquipmentByRoomId(Guid roomId);
        Task<List<LogEquipment>> GetLogEquipmentByName(string name);
        Task<List<LogEquipment>> GetLogEquipmentByCode(string code);
    }
}
