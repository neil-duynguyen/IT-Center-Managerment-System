using KidProEdu.Application.ViewModels.ConfigJobType;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Interfaces
{
    public interface IConfigJobTypeService
    {
        Task<List<ConfigJobType>> GetConfigJobTypes();
        Task<bool> UpdateConfigJobType(UpdateConfigJobTypeViewModel updateConfigJobTypeViewModel);
    }
}
