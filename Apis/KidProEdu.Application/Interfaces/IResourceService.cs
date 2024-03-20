using KidProEdu.Application.ViewModels.ResourceViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.ResourceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IResourceService
    {
        Task<List<ResourceViewModel>> GetResources();
        Task<bool> CreateResource(CreateResourceViewModel createResourceViewModel);
        Task<bool> UpdateResource(UpdateResourceViewModel updateResourceViewModel);
        Task<ResourceViewModel> GetResourceById(Guid id);
        Task<bool> DeleteResource(Guid id);
        Task<List<ResourceViewModel>> GetResourcesByCourseId(Guid id);
    }
}
