using KidProEdu.Application.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface ICourseService
    {
        public Task<bool> CreateCourseAsync(CreateCourseViewModel createCourseViewModel);
        public Task<bool> CreateCourseParentAsync(CreateCourseParentViewModel createCourseParentViewModel);
        Task<bool> UpdateCourseParentAsync(UpdateCourseParentViewModel updateCourseParentViewModel);
        Task<bool> UpdateCourseAsync(UpdateCourseViewModel updateCourseViewModel);
        public Task<List<CourseViewModel>> GetAllCourse();
        Task<List<CourseViewModel>> GetAllCourseInBlog();
        Task<List<CourseViewModel>> GetAllCourseSingle();
        public Task<List<CourseViewModel>> GetAllCourseByChildId(Guid childId);
        public Task<CourseViewModelById> GetCourseById(Guid Id);
        Task<bool> DeleteCourseAsync(Guid courseId);
        public Task<CourseSummariseDetailViewModel> CourseSummariseDetail(DateTime year);
    }
}
