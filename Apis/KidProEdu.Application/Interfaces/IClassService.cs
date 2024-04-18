using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Http;
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
        Task<List<ChildrenPassedViewModel>> ChangeStatusClass(ChangeStatusClassViewModel changeStatusClassViewModel);
        Task<List<ClassChildrenViewModel>> GetChildrenByClassId(Guid classId);
        Task<Stream> ExportExcelFileAsync(Guid classId);
        Task<bool> ImportScoreExcelFileAsync(IFormFile formFile);
        Task<bool> TestSendAttachEmail();
        Task<bool> ChangeTeacherForClass(ChangeTeacherForClassViewModel changeTeacherForClassViewModel);
        Task<List<ClassViewModel>> GetListClassTeachingByTeacher(Guid teacherId);
        Task<List<ClassViewModel>> GetListClassStatusPending();
    }
}
