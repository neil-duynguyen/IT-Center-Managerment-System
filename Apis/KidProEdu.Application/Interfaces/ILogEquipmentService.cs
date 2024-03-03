using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LogEquipmentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface ILogEquipmentService
    {
        Task<List<LogEquipmentViewModel>> GetLogEquipments();
        Task<bool> CreateLogEquipment(CreateLogEquipmentViewModel createLogEquipmentViewModel);
        Task<bool> UpdateLogEquipment(UpdateLogEquipmentViewModel updateLogEquipmentViewModel);
        Task<LogEquipmentViewModel> GetLogEquipmentById(Guid id);
        Task<bool> DeleteLogEquipment(Guid id);
        Task<List<LogEquipmentViewModel>> GetLogEquipmentsByRoomId(Guid roomId);
        Task<List<LogEquipmentViewModel>> GetLogEquipmentsByUserId(Guid userId);
        Task<List<LogEquipmentViewModel>> GetLogEquipmentsByName(string name);
        Task<List<LogEquipmentViewModel>> GetLogEquipmentsByEquipmentId(Guid equipmentId);
        Task<List<LogEquipmentViewModel>> GetLogEquipmentsByCode(string code);
    }
}
