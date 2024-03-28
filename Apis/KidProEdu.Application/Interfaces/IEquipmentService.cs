using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IEquipmentService
    {
        Task<List<EquipmentViewModel>> GetEquipments();
        Task<List<EquipmentViewModel>> GetListEquipmentByName(string name);
        Task<bool> CreateEquipment(CreateEquipmentViewModel createEquipmentViewModel);
        Task<bool> UpdateEquipment(UpdateEquipmentViewModel updateEquipmentViewModel);
        Task<EquipmentViewModel2> GetEquipmentById(Guid id);
        Task<bool> DeleteEquipment(Guid id);
        Task<bool> EquipmentBorrowedManagement(EquipmentWithLogEquipmentBorrowedViewModel equipmentWithLogEquipmentBorrowedViewModel);
        Task<bool> EquipmentRepairManagement(EquipmentWithLogEquipmentRepairViewModel equipmentWithLogEquipmentRepairViewModel);
        Task<bool> EquipmentReturnedManagement(EquipmentWithLogEquipmentReturnedViewModel equipmentWithLogEquipmentReturnedViewModel);
        Task AutoCheckReturn();
    }
}
