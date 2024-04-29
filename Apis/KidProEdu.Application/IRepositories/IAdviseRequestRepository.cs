using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

namespace KidProEdu.Application.IRepositories
{
    public interface IAdviseRequestRepository : IGenericRepository<AdviseRequest>
    {
        Task<AdviseRequest> GetAdviseRequestByEmail(string name);
        Task<AdviseRequest> GetAdviseRequestByPhone(string phone);
        Task<List<AdviseRequest>> GetAdviseRequestByTestDate(DateTime testDate);
        Task<AdviseRequest> GetAdviseRequestByProperty(UpdateAdviseRequestViewModel updateAdviseRequestViewModel, Expression<Func<AdviseRequest, object>> property);
        Task<List<AdviseRequest>> GetAdviseRequestByUserId(Guid id);
    }
}
