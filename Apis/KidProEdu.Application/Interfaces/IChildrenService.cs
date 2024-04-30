using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IChildrenService
    {
        Task<List<ChildrenViewModel>> GetChildrensByStaffId();
        Task<bool> CreateChildren(CreateChildrenViewModel createChildrenViewModel);
        Task<bool> UpdateChildren(UpdateChildrenViewModel updateChildrenViewModel);
        Task<bool> ChildrenReserve(ChildrenReserveViewModel childrenReserveViewModel);
        Task<bool> DeleteChildren(Guid childrenId);
        Task<ChildrenViewModel> GetChildrenById(Guid childrenId);
        Task<List<ChildrenViewModel>> GetChildrenByParentId(Guid Id);
        Task<ChildrenSummariseViewModel> GetChildrenSummariseViewModel(DateTime MonthAndYear);
        Task<List<CourseViewModel>> CourseSuggestions(Guid childrenId);

        Task<List<ChildrenViewModel>> GetListChildrenByOutClassId(Guid classId);
    }
}
