using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface ITagService
    {
        Task<List<TagViewModel>> GetBlogTags();
        Task<List<TagViewModel>> GetSkillTags();
        Task<bool> CreateTag(CreateTagViewModel createTagViewModel);
        Task<bool> UpdateTag(UpdateTagViewModel updateTagViewModel);
        Task<TagViewModel> GetTagById(Guid tagId);
        Task<bool> DeleteTag(Guid tagId);
    }
}
