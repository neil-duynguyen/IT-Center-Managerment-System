using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface IChildrenRepository : IGenericRepository<ChildrenProfile>
    {
        Task<List<ChildrenProfile>> GetChildrenProfiles(DateTime year);
        Task<List<ChildrenProfile>> GetListChildrenProfileByClassId(Guid classId);
        Task<int> GetTotalChildrens(DateTime startDate, DateTime endDate);
    }
}
