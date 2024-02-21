using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.RatingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface ILessonService
    {
        Task<List<LessonViewModel>> GetLessons();
        Task<bool> CreateLesson(CreateLessonViewModel createLessonViewModel);
        Task<bool> UpdateLesson(UpdateLessonViewModel updateLessonViewModel);
        Task<LessonViewModel> GetLessonById(Guid id);
        Task<bool> DeleteLesson(Guid id);
        Task<List<LessonViewModel>> GetLessonsByCourseId(Guid courseId);
    }
}
