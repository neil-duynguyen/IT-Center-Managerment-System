using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
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
        Task<List<CategoryEquipmentViewModel>> GetCategoryEquipments();
        Task<bool> CreateCategoryEquipment(CreateCategoryEquipmentViewModel createCategoryEquipmentViewModel);
        Task<bool> UpdateCategoryEquipment(UpdateCategoryEquipmentViewModel updateCategoryEquipmentViewModel);
        Task<CategoryEquipmentViewModel> GetCategoryEquipmentById(Guid id);
        Task<bool> DeleteCategoryEquipment(Guid id);
        Task<bool> BorrowCategoryEquipment(BorrowCategoryEquipmentViewModel borrowCategoryEquipmentViewModel);
        Task<bool> BorrowAutoCategoryEquipment(BorrowAutoCategoryEquipmentViewModel borrowAutoCategoryEquipmentViewModel);
        Task<bool> ReturnCategoryEquipment(ReturnCategoryEquipmentViewModel returnCategoryEquipmentViewModel);
    }
}
