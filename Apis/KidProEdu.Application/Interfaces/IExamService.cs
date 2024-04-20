using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IExamService
    {
        Task<List<ExamViewModel>> GetExams();
        Task<Exam> CreateExam(CreateExamViewModel2 createExamViewModel);
        Task<bool> UpdateExam(UpdateExamViewModel updateExamViewModel);
        Task<ExamViewModel> GetExamById(Guid id);
        Task<bool> DeleteExam(Guid id);
        Task<ExamViewModel> GetExamByTestName(string name);
        Task<List<ExamViewModel>> GetExamsByCourseId(Guid id);
        Task<List<ExamViewModel>> GetExamsByClassId(Guid id);
        Task<Exam> CreateExamFinalPractice(CreateExamFinalPracticeViewModel createExamViewModel);
    }
}
