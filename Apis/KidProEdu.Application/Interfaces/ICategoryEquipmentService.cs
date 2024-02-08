using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface ICategoryEquipmentService
    {
        Task<List<CategoryEquipment>> GetCategoryEquipments();
        Task<bool> CreateCategoryEquipment(CreateCategoryEquipmentViewModel createCategoryEquipmentViewModel);
        Task<bool> UpdateCategoryEquipment(UpdateCategoryEquipmentViewModel updateCategoryEquipmentViewModel);
        Task<CategoryEquipment> GetCategoryEquipmentById(Guid id);
        Task<bool> DeleteCategoryEquipment(Guid id);
    }
}
