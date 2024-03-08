using KidProEdu.Application.ViewModels.EnrollmentViewModels;
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
        Task<List<EnrollmentViewModel>> GetEnrollment();
        Task<List<EnrollmentViewModel>> GetEnrollmentById(Guid Id);
    }
}
