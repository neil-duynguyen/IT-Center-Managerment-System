using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<List<Attendance>> GetListAttendanceByClassIdAndDateAndScheduleId(Guid classId, DateTime date, Guid scheduleId);
        Task<List<Attendance>> GetListAttendanceByCourseIdAndChildId(Guid courseId, Guid childId);
        Task<List<Attendance>> GetAttendanceByScheduleId(Guid id);
        Task<Attendance> GetAttendanceByScheduleIdAndChilId(Guid scheduleId, Guid childId);
        Task<List<Attendance>> GetListAttendanceByScheduleIdAndChilId(Guid scheduleId, Guid childId);
        Task<List<Attendance>> GetListAttendanceByScheduleIdAndChilIdAndStatusFuture(Guid scheduleId, Guid childId);
        Task<List<Attendance>> GetListAttendanceByChilIdAndStatusFuture(Guid childId);
        Task<Attendance> GetListAttendanceByClassIdAndChilIdAndOutOfStatusFuture(Guid classId, Guid childId);
        Task<List<Attendance>> GetListAttendanceByClassIdAndChilIdOutOfStatusFuture(Guid classId, Guid childId);
        Task<List<Attendance>> GetAttendancesByChildId(Guid childId, DateTime startDate, DateTime endDate);
        Task<List<Attendance>> GetListAttendancesByChildId(Guid childId);
    }
}
