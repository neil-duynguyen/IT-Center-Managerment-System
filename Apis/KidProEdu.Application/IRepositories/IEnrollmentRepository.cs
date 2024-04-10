using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<List<Enrollment>> GetEnrollmentsByClassId(Guid Id);

        Task<Enrollment> GetEnrollmentsByClassIdAndChildrenProfileId(Guid classId, Guid childId);

        Task<List<Enrollment>> GetEnrollmentsByChildId(Guid Id);
        Task<double> GetCommissionEnrollmentsTotalAmount(DateTime startDate, DateTime endDate);
    }
}
