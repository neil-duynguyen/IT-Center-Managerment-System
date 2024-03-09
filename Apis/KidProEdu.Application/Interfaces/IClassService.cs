using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IClassService
    {
        Task<List<ClassViewModel>> GetClasses();
        Task<bool> CreateClass(CreateClassViewModel createClassViewModel);
        Task<bool> UpdateClass(UpdateClassViewModel updateClassViewModel);
        Task<ClassViewModel> GetClassById(Guid ClassId);
        Task<bool> DeleteClass(Guid ClassId);
        Task<bool> ChangeStatusClass(ChangeStatusClassViewModel changeStatusClassViewModel);
        Task<List<ClassViewModel>> GetClassBySemester(Guid id);
        Task<List<ClassChildrenViewModel>> GetChildrenByClassId(Guid classId);
    }
}
