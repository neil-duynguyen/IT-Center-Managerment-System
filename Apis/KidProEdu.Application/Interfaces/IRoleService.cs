
using KidProEdu.Application.ViewModels.RoleViewModels;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetRole();
        Task<bool> CreateRole(string roleView);
    }
}
