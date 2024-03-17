using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using KidProEdu.Domain.Entities;
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

        public async Task<List<AttendanceViewModel>> GetAttendanceByScheduleId(Guid id)
        {
            var result = await _unitOfWork.AttendanceRepository.GetAttendanceByScheduleId(id);
            var mapper = _mapper.Map<List<AttendanceViewModel>>(result);
            return mapper;
        }

        public async Task<List<AttendanceViewModel>> GetAttendances()
        {
            var result = await _unitOfWork.AttendanceRepository.GetAllAsync();
            var mapper = _mapper.Map<List<AttendanceViewModel>>(result);
            return mapper;
        }

        public async Task<bool> UpdateAttendances(List<UpdateAttendanceViewModel> updateAttendanceViewModel)
        {
            foreach (var updateAttendance in updateAttendanceViewModel)
            {
                var attendanceUpdate = await _unitOfWork.AttendanceRepository.GetByIdAsync(updateAttendance.Id);
                var mapper = _mapper.Map<Attendance>(attendanceUpdate);
                mapper.Id = updateAttendance.Id;
                mapper.StatusAttendance = updateAttendance.StatusAttendance;
                _unitOfWork.AttendanceRepository.Update(attendanceUpdate);
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật điểm danh thất bại");

        }
    }

}
