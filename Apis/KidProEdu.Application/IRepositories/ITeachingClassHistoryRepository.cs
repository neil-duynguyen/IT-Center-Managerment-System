using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface ITeachingClassHistoryRepository : IGenericRepository<TeachingClassHistory>
    {
        Task<List<TeachingClassHistory>> GetClassByTeacherId(Guid id);
        Task<List<TeachingClassHistory>> GetTeachingHistoryByClassId(Guid id);
        Task<List<TeachingClassHistory>> GetTeachingHistoryByStatus(TeachingStatus status); 
    }
}
