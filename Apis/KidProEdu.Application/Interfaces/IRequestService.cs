using KidProEdu.Application.ViewModels.RequestViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IRequestService
    {
        Task<List<RequestViewModel>> GetRequests();
        Task<bool> CreateRequest(CreateRequestViewModel createRequestViewModel);
        Task<bool> UpdateRequest(UpdateRequestViewModel updateRequestViewModel);
        Task<Request> GetRequestById(Guid requestId);
        Task<bool> DeleteRequest(Guid requestId);
        Task<bool> ChangeStatusRequest(ChangeStatusRequestViewModel changeStatusRequestViewModel);
    }
}
