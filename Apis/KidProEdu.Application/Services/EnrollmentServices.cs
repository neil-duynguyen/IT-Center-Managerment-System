using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Children;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class EnrollmentServices : IEnrollmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public EnrollmentServices(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateEnrollment(CreateEnrollmentViewModel createEnrollmentViewModel)
        {
            //check number children in class
            var getNumbderChildren = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);

            if (getNumbderChildren.ActualNumber == getNumbderChildren.MaxNumber) throw new Exception($"Lớp học {getNumbderChildren.ClassCode} đã đủ số lượng trẻ.");

            var getPriceClass = _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId).Result.Course.Price;

            //update ActualNumber in class
            var updateActualNumber = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);
            updateActualNumber.ActualNumber = getNumbderChildren.Enrollments.Count + 1;

            var mapper = _mapper.Map<Enrollment>(createEnrollmentViewModel);
            mapper.RegisterDate = _currentTime.GetCurrentTime();
            mapper.Commission = getPriceClass * 0.1;
            mapper.UserId = _claimsService.GetCurrentUserId;

            var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(createEnrollmentViewModel.ClassId);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(getNumbderChildren.CourseId);
            DateTime startDate = (DateTime)schedules.FirstOrDefault().StartDate;

            int slot = 0;
            while (slot < course.DurationTotal)
            {
                // Kiểm tra xem ngày hiện tại là ngày trong tuần đã chỉ định trong lịch trình hay không
                if (schedules.Any(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())))
                {
                    // Kiểm tra xem điểm danh cho ngày này đã được tạo hay chưa
                    var attendance = new CreateAttendanceViewModel
                    {
                        ScheduleId = schedules.FirstOrDefault(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())).Id,
                        ChildrenProfileId = createEnrollmentViewModel.ChildrenProfileId,
                        Date = startDate,
                        StatusAttendance = StatusAttendance.Future,
                        Note = ""
                    };

                    // Lưu danh sách điểm danh vào cơ sở dữ liệu
                    var attendanceEntity = _mapper.Map<Attendance>(attendance);
                    await _unitOfWork.AttendanceRepository.AddAsync(attendanceEntity);
                    slot++;
                }

                // Tăng ngày startdate lên 1 để chuyển sang ngày tiếp theo
                startDate = startDate.AddDays(1);

                // Kiểm tra xem số buổi học đã điểm danh có bằng tổng số buổi học không
                if (slot == course.DurationTotal)
                {
                    break; // Kết thúc vòng lặp nếu đã điểm danh đủ số buổi học
                }
            }


            await _unitOfWork.EnrollmentRepository.AddAsync(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Đăng kí thất bại.");
        }

        public async Task<bool> DeleteEnrollment(Guid idChildren)
        { 
            var getEnrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(idChildren);

            if (getEnrollment is null) throw new Exception("Không tìm thấy Enrollment");

            _unitOfWork.EnrollmentRepository.SoftRemove(getEnrollment);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }

        //Lấy những children mà staff đã đăng kí dành cho staff dùng để quản lý n children là staff chụi trách nhiệm quản lý
        public async Task<List<EnrollmentViewModel>> GetEnrollment()
        {
            var getEnrollment = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.UserId == _claimsService.GetCurrentUserId).ToList();

            var mapper = _mapper.Map<List<EnrollmentViewModel>>(getEnrollment);
            return mapper;
        }

        //dành cho manager
        public async Task<List<EnrollmentViewModel>> GetEnrollmentById(Guid Id)
        {
            var getEnrollment = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.UserId == Id).ToList();

            var mapper = _mapper.Map<List<EnrollmentViewModel>>(getEnrollment);
            return mapper;
        }

        public async Task<List<EnrollmentViewModel>> GetEnrollmentsByClassId(Guid Id)
        {
            var getEnrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByClassId(Id);
            var mapper = _mapper.Map<List<EnrollmentViewModel>>(getEnrollments);
            return mapper;
        }
    }
}
