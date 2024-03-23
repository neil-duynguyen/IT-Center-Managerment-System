using KidProEdu.Application.ViewModels.FeedBackViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IFeedbackService
    {
        Task<List<FeedBackViewModel>> GetFeedbacks();
        Task<bool> CreateFeedback(CreateFeedBackViewModel createFeedBackViewModel);
        Task<bool> UpdateFeedback(UpdateFeedBackViewModel updateFeedBackViewModel);
        Task<FeedBackViewModel> GetFeedbackById(Guid id);
        Task<bool> DeleteFeedback(Guid id);
        Task<List<FeedBackViewModel>> GetFeedbackByUserId(Guid userId);
        Task<List<FeedBackViewModel>> GetFeedbackByClassId(Guid classId);
    }
}
