using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IAttendanceService
    {
        Task<List<AttendanceViewModel>> GetAttendanceByScheduleId(Guid id);
        Task<List<AttendanceViewModel>> GetAttendances();
        Task<bool> CreateAttendances(List<CreateAttendanceViewModel> createAttendanceViewModel);
        Task<bool> UpdateAttendances(List<UpdateAttendanceViewModel> updateAttendanceViewModel);
        Task<bool> DeleteAttendance(Guid id);
        Task<List<AttendanceDetailsViewModel>> GetAttendanceDetailsByCourseIdAndChildrenId(Guid courseId, Guid childId);
        Task<List<AttendanceWithChildrenProfileViewModel>> GetListAttendanceByClassIdAndDateAndScheduleId(Guid classId, DateTime date, Guid scheduleId);
        //Task<List<AttendanceViewModel>> GetAttendanceDetailsByCourseIdAndChildrenId(Guid courseId, Guid childId);
        Task<List<AttendanceDetailsViewModel>> GetAttendancesByChildId(Guid childId, DateTime startDate, DateTime endDate);
    }
}
