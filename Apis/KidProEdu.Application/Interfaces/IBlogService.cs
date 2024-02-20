using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IBlogService
    {
        Task<List<BlogViewModel>> GetBlogs();
        Task<bool> CreateBlog(CreateBlogViewModel createBlogViewModel);
        Task<bool> UpdateBlog(UpdateBlogViewModel updateBlogViewModel);
        Task<BlogViewModel> GetBlogById(Guid id);
        Task<bool> DeleteBlog(Guid id);
        Task<List<BlogViewModel>> GetBlogWithUserByBlogId(Guid id);
    }
}
