using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IEnrollmentServices
    {
        Task<List<string>> CreateEnrollment(CreateEnrollmentViewModel createEnrollmentViewModel);
        Task<bool> UpdateEnrollment(UpdateEnrollmentViewModel updateEnrollmentViewModel);
        Task<bool> UpdateEnrollmentStudying(UpdateEnrollmentViewModel updateEnrollmentViewModel);
        Task<List<EnrollmentViewModel>> GetEnrollment();
        Task<List<EnrollmentViewModel>> GetEnrollmentById(Guid Id);
        Task<List<EnrollmentViewModel>> GetEnrollmentsByClassId(Guid Id);
        Task<bool> DeleteEnrollment(Guid classId, Guid childId);
        Task<bool> ImportExcelFile(IFormFile formFile);
    };
}
