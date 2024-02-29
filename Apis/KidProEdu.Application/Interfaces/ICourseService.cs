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
        //public Task<List<CourseViewModel>> GetAllCourse();
        Task<bool> DeleteCourseAsync(Guid courseId);
    }
}
