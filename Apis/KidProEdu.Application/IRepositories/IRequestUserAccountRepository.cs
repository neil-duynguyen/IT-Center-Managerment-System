using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface IRequestUserAccountRepository : IGenericRepository<RequestUserAccount>
    {
        Task<List<RequestUserAccount>> GetRequestUserByRequestId(Guid requestId);
        Task<List<RequestUserAccount>> GetRequestUserByRecieverId(Guid recieverId);
    }
}
