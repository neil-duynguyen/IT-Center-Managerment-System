using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<bool> CheckNameExited(string name);
        Task<List<Course>> GetListCourseByChildrenId(Guid childrenId);
        Task<int> GetTotalCourses(DateTime startDate, DateTime endDate);
    }
}
