using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KidProEdu.Application.Services.CategoryEquipmentService;

namespace KidProEdu.Application.Interfaces
{

    public interface ICategoryEquipmentService
    {
        Task<List<CategoryEquipmentViewModel>> GetCategoryEquipments();
        Task<bool> CreateCategoryEquipment(CreateCategoryEquipmentViewModel createCategoryEquipmentViewModel);
        Task<bool> UpdateCategoryEquipment(UpdateCategoryEquipmentViewModel updateCategoryEquipmentViewModel);
        Task<CategoryEquipmentViewModel> GetCategoryEquipmentById(Guid id);
        Task<bool> DeleteCategoryEquipment(Guid id);
        Task<bool> BorrowWithStaffCategoryEquipment(List<BorrowCategoryEquipmentViewModel> borrowCategoryEquipmentViewModels);
        Task<bool> BorrowCategoryEquipment(List<BorrowAutoCategoryEquipmentViewModel> borrowCategoryEquipmentViewModels);
        Task<bool> ReturnCategoryEquipment(List<ReturnCategoryEquipmentViewModel> returnCategoryEquipmentViewModels);
        Task<bool> ReturnForHomeCategoryEquipment(List<ReturnCategoryEquipmentViewModel> returnCategoryEquipmentViewModels);
        Task<bool> BorrowForGoHomeCategoryEquipment(List<BorrowForGoHomeCategoryEquipmentViewModel> borrowForGoHomeCategoryEquipmentViewModels);
        Task UpdateQuantityEquipment(UpdateQuantityCategoryEquipment updateQuantityCategory);
        Task<List<EquipmentReportViewModel>> EquipmentReport();
    }
}
