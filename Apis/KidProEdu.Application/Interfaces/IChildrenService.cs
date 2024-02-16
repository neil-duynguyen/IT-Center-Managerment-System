using KidProEdu.Application.ViewModels.ChildrenViewModels;
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
        Task<List<ChildrenProfile>> GetChildrens();
        Task<bool> CreateChildren(CreateChildrenViewModel createChildrenViewModel);
        Task<bool> UpdateChildren(UpdateChildrenViewModel updateChildrenViewModel);
        Task<Location> GetChildrenById(Guid childrenId);
        Task<bool> DeleteChildren(Guid childrenId);
    }
}
