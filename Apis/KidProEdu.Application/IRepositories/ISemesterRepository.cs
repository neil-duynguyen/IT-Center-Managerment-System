using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface ISemesterRepository : IGenericRepository<Semester>
    {
        Task<List<Semester>> GetSemesterBySemesterName(string SemesterName);
        Task<List<Semester>> GetSemesterByStartDate(DateTime startDate);
    }
}
