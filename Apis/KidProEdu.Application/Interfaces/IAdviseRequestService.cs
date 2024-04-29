using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

namespace KidProEdu.Application.Interfaces
{
    public interface IAdviseRequestService
    {
        Task<List<AdviseRequestViewModel>> GetAdviseRequests();
        Task<bool> CreateAdviseRequest(CreateAdviseRequestViewModel createAdviseRequestViewModel);
        Task<bool> UpdateAdviseRequest(UpdateAdviseRequestViewModel updateAdviseRequestViewModel, params Expression<Func<AdviseRequest, object>>[] uniqueProperties);
        Task<AdviseRequestViewModel> GetAdviseRequestById(Guid AdviseRequestId);
        Task<List<AdviseRequestViewModel>> GetAdviseRequestByTestDate(DateTime testDate);
        Task<bool> DeleteAdviseRequest(Guid AdviseRequestId);
        Task AutoSendEmail();
        Task<List<AdviseRequestViewModel>> GetAdviseRequestByUserId(Guid id);
    }
}
