using KidProEdu.Application.ViewModels.SemesterViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface ISemesterService
    {
        Task<List<Semester>> GetSemesters();
        Task<bool> CreateSemester(CreateSemesterViewModel createSemesterViewModel);
        Task<bool> UpdateSemester(UpdateSemesterViewModel updateSemesterViewModel);
        Task<Semester> GetSemesterById(Guid semesterId);
        Task<bool> DeleteSemester(Guid semesterId);
    }
}
