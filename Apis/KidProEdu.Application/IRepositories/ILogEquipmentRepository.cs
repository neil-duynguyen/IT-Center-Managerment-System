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

    public interface ILogEquipmentRepository : IGenericRepository<LogEquipment>
    {
        Task<List<LogEquipment>> GetLogEquipmentByUserId(Guid userId);
        Task<List<LogEquipment>> GetLogEquipmentByEquipmentId(Guid equipmentId);
        Task<List<LogEquipment>> GetLogEquipmentByRoomId(Guid roomId);
        Task<List<LogEquipment>> GetLogEquipmentByName(string name);
        Task<List<LogEquipment>> GetLogEquipmentByCode(string code);
        Task<List<LogEquipment>> GetLogEquipmentByStatus(StatusOfEquipment statusOfEquipment);
        Task<List<LogEquipment>> GetLogEquipmentByReturnDeadline(DateTime returnDeadline);
        Task<List<LogEquipment>> GetLogEquipmentsByCateEquipmentId(Guid cateId);
        Task<LogEquipment> GetLogEquipmentByEquipmentIdAndUserAccountIdAndLogTypeAtClass(Guid equipmentId, Guid UserId, LogType logType);
        Task<LogEquipment> GetLogEquipmentByEquipmentIdAndUserAccountIdAndLogTypeAtHome(Guid equipmentId, Guid UserId, LogType logType);
    }
}
