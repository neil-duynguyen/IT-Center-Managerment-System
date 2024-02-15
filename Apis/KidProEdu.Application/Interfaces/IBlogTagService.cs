using KidProEdu.Application.ViewModels.BlogTagViewModels;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IBlogTagService
    {
        Task<List<BlogTag>> GetBlogTags();
        Task<bool> CreateBlogTag(CreateBlogTagViewModel createBlogTagViewModel);
        Task<bool> UpdateBlogTag(UpdateBlogTagViewModel updateBlogTagViewModel);
        Task<BlogTag> GetBlogTagById(Guid id);
        Task<bool> DeleteBlogTag(Guid id);
    }
}
