using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface ISemesterRepository : IGenericRepository<Semester>
    {
        Task<Semester> GetSemesterBySemesterName(string SemesterName);
        Task<Semester> GetSemesterByStartDate(DateTime startDate);
        Task<Semester> GetSemesterByProperty(UpdateSemesterViewModel updateSemesterViewModel, Expression<Func<Semester, object>> property);
    }
}
