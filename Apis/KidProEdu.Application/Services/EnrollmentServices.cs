using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Children;
using KidProEdu.Application.Validations.Enrollments;
using KidProEdu.Application.Validations.SkillCertificates;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
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
            //check enrolled
            var enrolled = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByClassIdAndChildrenProfileId(createEnrollmentViewModel.ClassId, createEnrollmentViewModel.ChildrenProfileId);
            if (enrolled != null)
            {
                throw new Exception("Bạn đã tham gia lớp này rồi!");
            }


            //check number children in class
            var getNumbderChildren = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);

            if (getNumbderChildren.ActualNumber == getNumbderChildren.MaxNumber) throw new Exception($"Lớp học {getNumbderChildren.ClassCode} đã đủ số lượng trẻ.");

            var getPriceClass = _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId).Result.Course.Price;

            //update ActualNumber in class
            var updateActualNumberClass = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);
            updateActualNumberClass.ActualNumber += 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberClass);

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

        public async Task<bool> DeleteEnrollment(Guid classId, Guid childId)
        {
            var getEnrollment = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByClassIdAndChildrenProfileId(classId, childId);
            if (getEnrollment == null)
            {
                throw new Exception("Không tìm thấy tham gia của học sinh ở lớp này");
            }
            else
            {
                //update ActualNumber in class
                var updateActualNumberClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
                updateActualNumberClass.ActualNumber = updateActualNumberClass.ActualNumber - 1;
                _unitOfWork.ClassRepository.Update(updateActualNumberClass);

                //delete attendance 
                var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(classId);
                foreach (var schedule in schedules)
                {
                    var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilId(schedule.Id, childId);
                    _unitOfWork.AttendanceRepository.RemoveRange(attendances);
                    await _unitOfWork.SaveChangeAsync();
                }

                //delete enrolled
                _unitOfWork.EnrollmentRepository.SoftRemove(getEnrollment);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
            }


        }

        /*public async Task<bool> DeleteEnrollment(Guid idChildren)
        { 
            var getEnrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(idChildren);

            if (getEnrollment is null) throw new Exception("Không tìm thấy Enrollment");

            _unitOfWork.EnrollmentRepository.SoftRemove(getEnrollment);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        }*/

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

        public async Task<bool> UpdateEnrollment(UpdateEnrollmentViewModel updateEnrollmentViewModel)
        {
            var validator = new UpdateEnrollmentViewModelValidator();
            var validationResult = validator.Validate(updateEnrollmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var result = await _unitOfWork.EnrollmentRepository.GetByIdAsync(updateEnrollmentViewModel.Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy tham gia này");
            }

            if(result.ClassId == updateEnrollmentViewModel.ClassId)
            {
                throw new Exception("Bạn đang học lớp này");
            }


            //delete attendance old schedule
            var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.ClassId);
            foreach(var schedule in schedules)
            {
                var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilId(schedule.Id, result.ChildrenProfileId);
                _unitOfWork.AttendanceRepository.RemoveRange(attendances);
                await _unitOfWork.SaveChangeAsync();
            }

            //update ActualNumber in new class
            var updateActualNumberNewClass = await _unitOfWork.ClassRepository.GetByIdAsync(updateEnrollmentViewModel.ClassId);
            updateActualNumberNewClass.ActualNumber = updateActualNumberNewClass.ActualNumber + 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberNewClass);

            //update ActualNumber in old class
            var updateActualNumberOldClass = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            updateActualNumberOldClass.ActualNumber = updateActualNumberOldClass.ActualNumber - 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberOldClass);

            var mapper = _mapper.Map<Enrollment>(result);
            mapper.ClassId = updateEnrollmentViewModel.ClassId;
            mapper.ChildrenProfileId = result.ChildrenProfileId;
            var schedules1 = await _unitOfWork.ScheduleRepository.GetScheduleByClass(mapper.ClassId);
            var classed = await _unitOfWork.ClassRepository.GetByIdAsync(mapper.ClassId);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(classed.CourseId);
            DateTime startDate = (DateTime)schedules1.FirstOrDefault().StartDate;

            int slot = 0;
            while (slot < course.DurationTotal)
            {
                // Kiểm tra xem ngày hiện tại là ngày trong tuần đã chỉ định trong lịch trình hay không
                if (schedules1.Any(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())))
                {
                    // Kiểm tra xem điểm danh cho ngày này đã được tạo hay chưa
                    var attendance = new CreateAttendanceViewModel
                    {
                        ScheduleId = schedules1.FirstOrDefault(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())).Id,
                        ChildrenProfileId = mapper.ChildrenProfileId,
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


            _unitOfWork.EnrollmentRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật tham gia này thất bại");
        }

        public async Task<bool> UpdateEnrollmentStudying(UpdateEnrollmentViewModel updateEnrollmentViewModel)
        {
            var validator = new UpdateEnrollmentViewModelValidator();
            var validationResult = validator.Validate(updateEnrollmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var result = await _unitOfWork.EnrollmentRepository.GetByIdAsync(updateEnrollmentViewModel.Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy tham gia này");
            }
            int slot = 0;

            //delete old schedule chua hoc
            var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.ClassId);
            foreach (var schedule in schedules)
            {
                var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilIdAndStatusFuture(schedule.Id, result.ChildrenProfileId);
                slot = attendances.Count();
                _unitOfWork.AttendanceRepository.RemoveRange(attendances);
                await _unitOfWork.SaveChangeAsync();
            }

            //update ActualNumber in new class
            var updateActualNumberNewClass = await _unitOfWork.ClassRepository.GetByIdAsync(updateEnrollmentViewModel.ClassId);
            updateActualNumberNewClass.ActualNumber = updateActualNumberNewClass.ActualNumber + 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberNewClass);

            //update ActualNumber in old class
            var updateActualNumberOldClass = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            updateActualNumberOldClass.ActualNumber = updateActualNumberOldClass.ActualNumber - 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberOldClass);

            var attendanceCheckTime = await _unitOfWork.AttendanceRepository.GetListAttendanceByClassIdAndChilIdAndOutOfStatusFuture(result.ClassId, result.ChildrenProfileId);
            result.ClassId = updateEnrollmentViewModel.ClassId;
            var schedules1 = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.ClassId);
            var classed = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(classed.CourseId);            
            DateTime startDate = attendanceCheckTime.Date;

            //add new schedule 
            int slot1 = 0;
            while (slot1 < course.DurationTotal - slot)
            {
                // Kiểm tra xem ngày hiện tại là ngày trong tuần đã chỉ định trong lịch trình hay không
                if (schedules1.Any(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())))
                {
                    // Kiểm tra xem điểm danh cho ngày này đã được tạo hay chưa
                    var attendance = new CreateAttendanceViewModel
                    {
                        ScheduleId = schedules1.FirstOrDefault(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())).Id,
                        ChildrenProfileId = result.ChildrenProfileId,
                        Date = startDate,
                        StatusAttendance = StatusAttendance.Future,
                        Note = ""
                    };

                    // Lưu danh sách điểm danh vào cơ sở dữ liệu
                    var attendanceEntity = _mapper.Map<Attendance>(attendance);
                    await _unitOfWork.AttendanceRepository.AddAsync(attendanceEntity);
                    slot1++;
                }

                // Tăng ngày startdate lên 1 để chuyển sang ngày tiếp theo
                startDate = startDate.AddDays(1);

                // Kiểm tra xem số buổi học đã điểm danh có bằng tổng số buổi học không
                if (slot1 == course.DurationTotal)
                {
                    break; // Kết thúc vòng lặp nếu đã điểm danh đủ số buổi học
                }
            }


            _unitOfWork.EnrollmentRepository.Update(result);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật tham gia này thất bại");
        }
    }
}
