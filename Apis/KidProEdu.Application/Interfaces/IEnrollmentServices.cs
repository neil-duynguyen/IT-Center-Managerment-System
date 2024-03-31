using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IEnrollmentServices
    {
        Task<bool> CreateEnrollment(CreateEnrollmentViewModel createEnrollmentViewModel);
        Task<bool> UpdateEnrollment(UpdateEnrollmentViewModel updateEnrollmentViewModel);
        Task<bool> UpdateEnrollmentStudying(UpdateEnrollmentViewModel updateEnrollmentViewModel);
        Task<List<EnrollmentViewModel>> GetEnrollment();
        Task<List<EnrollmentViewModel>> GetEnrollmentById(Guid Id);
        Task<List<EnrollmentViewModel>> GetEnrollmentsByClassId(Guid Id);
    }
}
