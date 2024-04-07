using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<List<Schedule>> GetScheduleByClass(Guid classId);
        Task<Schedule> GetScheduleBySlot(Guid slotId);
        Task<List<Schedule>> GetListScheduleBySlot(Guid slotId);
    }
}
