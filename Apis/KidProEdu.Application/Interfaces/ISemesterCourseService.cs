using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.SemesterCourseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface ISemesterCourseService
    {
        Task<bool> AddCourse(CreateSemesterCourseViewModel createSemesterCourseView);
        Task<List<CourseViewModel>> GetSemesterCourseById(Guid Id);
        Task<bool> DeleteSemesterCourse(Guid courseId);
    }
}
