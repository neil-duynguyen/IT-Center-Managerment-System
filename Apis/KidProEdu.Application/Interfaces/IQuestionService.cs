using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<List<Question2ViewModel>> GetQuestions();
        Task<List<Question2ViewModel>> GetQuestionsByType(QuestionType type);
        Task<bool> CreateQuestion(CreateQuestionViewModel[] createQuestionViewModel);
        Task<bool> UpdateQuestion(UpdateQuestionViewModel updateQuestionViewModel);
        Task<Question> GetQuestionById(Guid questionId);
        Task<List<QuestionByLessonViewModel>> CreateTest(List<CreateExamViewModel> createExamViewModels);
        Task<bool> DeleteQuestion(Guid questionId);
        Task<List<Question>> CreateTestEntry(CreateExamEntryViewModel createExamEntryViewModel);
    }
}
