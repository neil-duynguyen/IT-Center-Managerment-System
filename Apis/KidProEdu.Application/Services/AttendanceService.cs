using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Attendances;
using KidProEdu.Application.Validations.SkillCertificates;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace KidProEdu.Application.Services
{

    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public AttendanceService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateAttendances(List<CreateAttendanceViewModel> createAttendanceViewModel)
        {           
            var validator = new CreateAttendanceViewModelValidator();
            foreach (var attendanceViewModel in createAttendanceViewModel)
            {
                var validationResult = validator.Validate(attendanceViewModel);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }
            }
            var mapper = _mapper.Map<List<Attendance>>(createAttendanceViewModel);
            await _unitOfWork.AttendanceRepository.AddRangeAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo điểm danh thất bại");
        }

        public async Task<bool> DeleteAttendance(Guid id)
        {
            var result = await _unitOfWork.AttendanceRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy điểm danh này");
            else
            {
                _unitOfWork.AttendanceRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa điểm danh thất bại");
            }
        }

        public async Task<List<AttendanceViewModel>> GetAttendanceByScheduleId(Guid id)
        {
            var result = await _unitOfWork.AttendanceRepository.GetAttendanceByScheduleId(id);
            var mapper = _mapper.Map<List<AttendanceViewModel>>(result);
            return mapper;
        }

        public async Task<List<AttendanceDetailsViewModel>> GetAttendanceDetailsByCourseIdAndChildrenId(Guid courseId, Guid childId)
        {
            var result = await _unitOfWork.AttendanceRepository.GetListAttendanceByCourseIdAndChildId(courseId, childId);
            var mapper = _mapper.Map<List<AttendanceDetailsViewModel>>(result);
            return mapper;
        }

        public async Task<List<AttendanceViewModel>> GetAttendances()
        {
            var result = await _unitOfWork.AttendanceRepository.GetAllAsync();
            var mapper = _mapper.Map<List<AttendanceViewModel>>(result);
            return mapper;
        }

        public async Task<List<AttendanceDetailsViewModel>> GetAttendancesByChildId(Guid childId, DateTime startDate, DateTime endDate)
        {
            var result = await _unitOfWork.AttendanceRepository.GetAttendancesByChildId(childId, startDate, endDate);
            var mapper = _mapper.Map<List<AttendanceDetailsViewModel>>(result);
            return mapper;
        }

        public async Task<List<AttendanceWithChildrenProfileViewModel>> GetListAttendanceByClassIdAndDateAndScheduleId(Guid classId, DateTime date, Guid scheduleId)
        {
            var result = await _unitOfWork.AttendanceRepository.GetListAttendanceByClassIdAndDateAndScheduleId(classId, date, scheduleId);
            var mapper = _mapper.Map<List<AttendanceWithChildrenProfileViewModel>>(result);
            return mapper;
        }

        public async Task<bool> UpdateAttendances(List<UpdateAttendanceViewModel> updateAttendanceViewModel)
        {
            foreach (var updateAttendance in updateAttendanceViewModel)
            {
                var validator = new UpdateAttendanceViewModelValidator();
                var validationResult = validator.Validate(updateAttendance);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }  

                var attendanceUpdate = await _unitOfWork.AttendanceRepository.GetByIdAsync(updateAttendance.Id);
                attendanceUpdate.StatusAttendance = updateAttendance.StatusAttendance;
                attendanceUpdate.Note = updateAttendance.Note;
                if(attendanceUpdate.Date.Date > _currentTime.GetCurrentTime().Date)
                {
                    throw new Exception("Chưa tới ngày học để điểm danh.");
                }
                _unitOfWork.AttendanceRepository.Update(attendanceUpdate);
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật điểm danh thất bại");

        }
    }

}
