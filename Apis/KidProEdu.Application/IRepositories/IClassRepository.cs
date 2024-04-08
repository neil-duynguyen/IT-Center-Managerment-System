using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<List<Class>> GetClassByClassCode(string classCode);
        Task<List<Class>> GetClassBySlot(int slot);
        Task<List<Class>> GetClassByCourseId(Guid id, DateTime year);
    }
}
