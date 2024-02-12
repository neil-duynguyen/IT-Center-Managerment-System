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
        Task<List<Equipment>> GetEquipments();
        Task<bool> CreateEquipment(CreateEquipmentViewModel createEquipmentViewModel);
        Task<bool> UpdateEquipment(UpdateEquipmentViewModel updateEquipmentViewModel);
        Task<Equipment> GetEquipmentById(Guid id);
        Task<bool> DeleteEquipment(Guid id);
    }
}
