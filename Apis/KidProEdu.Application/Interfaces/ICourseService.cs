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
        public Task<List<CourseViewModel>> GetAllCourse();
        public Task<CourseViewModel> GetCourseById(Guid Id);
        Task<bool> DeleteCourseAsync(Guid courseId);
    }
}
