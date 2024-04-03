using KidProEdu.Application.ViewModels.ChildrenAnswerViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IChildrenAnswerService
    {
        Task<List<QuestionViewModel>> GetChildrenAnswers(Guid childrenId, Guid examId);
        Task<bool> CreateChildrenAnswers(List<CreateChildrenAnswerViewModel> createChildrenAnswerViewModel);
        Task<bool> UpdateChildrenAnswer(UpdateChildrenAnswerViewModel updateChildrenAnswerView);
        Task<ChildrenAnswerViewModel> GetChildrenAnswerById(Guid id);
        Task<bool> DeleteChildrenAnswer(Guid id);
    }
}
