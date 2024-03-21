using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface IExamRepository : IGenericRepository<Exam>
    {
        Task<Exam> GetExamByTestName(string name);
        Task<List<Exam>> GetExamByCourseId(Guid id);
    }
}
