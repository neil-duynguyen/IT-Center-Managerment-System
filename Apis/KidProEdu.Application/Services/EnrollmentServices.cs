using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Hangfire.Logging;
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
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<List<string>> CreateEnrollment(CreateEnrollmentViewModel createEnrollmentViewModel)
        {
            List<string> failedEnrollments = new List<string>();


     
            var getNumberChildren = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);
     
            int totalChildren = createEnrollmentViewModel.ChildrenProfileIds.Count;
            if (totalChildren > (getNumberChildren.MaxNumber - getNumberChildren.ActualNumber))
            {
                throw new Exception($"Số lượng trẻ đăng ký vượt quá số lượng còn trống trong lớp {getNumberChildren.ClassCode}.");
            }

      
            if (getNumberChildren.StatusOfClass == StatusOfClass.Started)
            {
                throw new Exception($"Lớp {getNumberChildren.ClassCode} đã bắt đầu.");
            }

            //check class cancel
            if (getNumberChildren.StatusOfClass == StatusOfClass.Cancel)
            {
                throw new Exception($"Lớp {getNumberChildren.ClassCode} đã bị hủy.");
            }

            //check class Expired
            if (getNumberChildren.StatusOfClass == StatusOfClass.Expired)
            {
                throw new Exception($"Lớp {getNumberChildren.ClassCode} đã hết hạn.");
            }

            foreach (var childId in createEnrollmentViewModel.ChildrenProfileIds)
            {
                try
                {
                    var enrolled = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByClassIdAndChildrenProfileId(createEnrollmentViewModel.ClassId, childId);
                    if (enrolled != null)
                    {
                        var child = await _unitOfWork.ChildrenRepository.GetByIdAsync(enrolled.ChildrenProfileId);
                        failedEnrollments.Add($"{child.FullName} đã tham gia lớp này rồi!");
                    }

                    var getPriceClass = _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId).Result.Course.Price;

                 
                    var updateActualNumberClass = await _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId);
                    updateActualNumberClass.ActualNumber += 1;
                    _unitOfWork.ClassRepository.Update(updateActualNumberClass);

         
                    var createNewEnroll = new Enrollment();
                    createNewEnroll.ChildrenProfileId = childId;
                    createNewEnroll.ClassId = createEnrollmentViewModel.ClassId;
                  
                    var mapper = _mapper.Map<Enrollment>(createNewEnroll);
                    mapper.RegisterDate = _currentTime.GetCurrentTime();
                    mapper.Commission = getPriceClass * 0.1;
                    mapper.UserId = _claimsService.GetCurrentUserId;

                 
                    var children = await _unitOfWork.ChildrenRepository.GetByIdAsync(childId);
                    children.Status = StatusChildrenProfile.Studying;
                    _unitOfWork.ChildrenRepository.Update(children);

                    var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(createEnrollmentViewModel.ClassId);
                    var getCourseId = _unitOfWork.ClassRepository.GetByIdAsync(createEnrollmentViewModel.ClassId).Result.CourseId;
                    var course = await _unitOfWork.CourseRepository.GetByIdAsync(getCourseId);
                    DateTime startDate = (DateTime)schedules.FirstOrDefault().StartDate;

                    int slot = 0;
                    bool duplicateScheduleError = false;
                    while (slot < course.DurationTotal)
                    {
                        
                        if (schedules.Any(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())))
                        {
                           
                            var attendance = new CreateAttendanceViewModel
                            {
                                ScheduleId = schedules.FirstOrDefault(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())).Id,
                                ChildrenProfileId = childId,
                                Date = startDate,
                                StatusAttendance = StatusAttendance.Future,
                                Note = ""
                            };

                            var attendances = await _unitOfWork.AttendanceRepository.GetListAttendancesByChildId(childId);
                            foreach (var atten in attendances)
                            {
                                if (atten.Date.Date == startDate.Date)
                                {
                                    if (schedules.Any(x => x.SlotId.ToString().Contains(atten.Schedule.SlotId.ToString())))
                                    {
                                        if (!duplicateScheduleError)
                                        {
                                            var child = await _unitOfWork.ChildrenRepository.GetByIdAsync(childId);
                                            failedEnrollments.Add($"{child.FullName} có lịch học bị trùng với lịch khác");
                                            duplicateScheduleError = true;
                                        }
                                    }
                                }
                                if (duplicateScheduleError == true)
                                {
                                    break;
                                }
                            }

                            
                            var attendanceEntity = _mapper.Map<Attendance>(attendance);
                            await _unitOfWork.AttendanceRepository.AddAsync(attendanceEntity);
                            slot++;
                        }
                    
                        startDate = startDate.AddDays(1);

                       
                        if (slot == course.DurationTotal)
                        {
                            break; 
                        }
                    }
                    await _unitOfWork.EnrollmentRepository.AddAsync(mapper);
                }
                catch (Exception ex)
                {
                    failedEnrollments.Add(ex.Message);
                }
            }
            if (failedEnrollments.Any())
            {
                return failedEnrollments;
            }
            else
            {
                await _unitOfWork.SaveChangeAsync();
                return new List<string>();
            }

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
                var updateActualNumberClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
                updateActualNumberClass.ActualNumber = updateActualNumberClass.ActualNumber - 1;
                _unitOfWork.ClassRepository.Update(updateActualNumberClass);

                var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(classId);
                foreach (var schedule in schedules)
                {
                    var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilId(schedule.Id, childId);
                    _unitOfWork.AttendanceRepository.RemoveRange(attendances);
                }

                var getEnrollments = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByChildId(childId);
                if (getEnrollments.Count <= 1)
                {
                    var children = await _unitOfWork.ChildrenRepository.GetByIdAsync(childId);
                    children.Status = StatusChildrenProfile.Waiting;
                    _unitOfWork.ChildrenRepository.Update(children);
                }
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

            if (result.ClassId == updateEnrollmentViewModel.ClassId)
            {
                throw new Exception("Bạn đang học lớp này");
            }

            var checkCourse = await _unitOfWork.ClassRepository.GetByIdAsync(updateEnrollmentViewModel.ClassId);
            var oldCourse = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            if (checkCourse.CourseId != oldCourse.CourseId)
            {
                throw new Exception("Lớp học mới có khóa học khác với lớp hiện tại");
            }

            if (checkCourse.ActualNumber >= checkCourse.MaxNumber)
            {
                throw new Exception("Lớp học mới đã có sĩ số tối đa");
            }

            var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.ClassId);
            foreach (var schedule in schedules)
            {
                var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilId(schedule.Id, result.ChildrenProfileId);
                _unitOfWork.AttendanceRepository.RemoveRange(attendances);
            }

            var updateActualNumberNewClass = await _unitOfWork.ClassRepository.GetByIdAsync(updateEnrollmentViewModel.ClassId);
            updateActualNumberNewClass.ActualNumber = updateActualNumberNewClass.ActualNumber + 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberNewClass);

            var updateActualNumberOldClass = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            updateActualNumberOldClass.ActualNumber = updateActualNumberOldClass.ActualNumber - 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberOldClass);

            result.ClassId = updateEnrollmentViewModel.ClassId;
            result.ChildrenProfileId = result.ChildrenProfileId;
            var schedules1 = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.ClassId);
            var classed = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(classed.CourseId);
            DateTime startDate = (DateTime)schedules1.FirstOrDefault().StartDate;

            int slot = 0;
            while (slot < course.DurationTotal)
            {            
                if (schedules1.Any(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())))
                {
                    var attendance = new CreateAttendanceViewModel
                    {
                        ScheduleId = schedules1.FirstOrDefault(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())).Id,
                        ChildrenProfileId = result.ChildrenProfileId,
                        Date = startDate,
                        StatusAttendance = StatusAttendance.Future,
                        Note = ""
                    };

                    var attendances = await _unitOfWork.AttendanceRepository.GetListAttendancesByChildId(result.ChildrenProfileId);
                    foreach (var atten in attendances)
                    {
                        if (atten.Date.Date == startDate.Date)
                        {
                            if (schedules1.Any(x => x.SlotId.ToString().Contains(atten.Schedule.SlotId.ToString())))
                            {
                                throw new Exception("Lớp học này có lịch trùng với lịch học bạn đã đăng kí ");
                            }
                        }
                    }

                    var attendanceEntity = _mapper.Map<Attendance>(attendance);
                    await _unitOfWork.AttendanceRepository.AddAsync(attendanceEntity);
                    slot++;
                }

                startDate = startDate.AddDays(1);

              
                if (slot == course.DurationTotal)
                {
                    break;
                }
            }


            _unitOfWork.EnrollmentRepository.Update(result);
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

            if (result.ClassId == updateEnrollmentViewModel.ClassId)
            {
                throw new Exception("Bạn đang học lớp này");
            }

            int slot = 0;

            var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.ClassId);
            foreach (var schedule in schedules)
            {
                var attendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilIdAndStatusFuture(schedule.Id, result.ChildrenProfileId);
                slot = attendances.Count();
                _unitOfWork.AttendanceRepository.RemoveRange(attendances);
            }

            var checkCourse = await _unitOfWork.ClassRepository.GetByIdAsync(updateEnrollmentViewModel.ClassId);
            var oldCourse = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
           
            if (checkCourse.CourseId != oldCourse.CourseId)
            {
                throw new Exception("Lớp học mới có khóa học khác với lớp hiện tại");
            }

            if(checkCourse.ActualNumber >= checkCourse.MaxNumber)
            {
                throw new Exception("Lớp học mới đã có sĩ số tối đa");
            }

            var updateActualNumberNewClass = await _unitOfWork.ClassRepository.GetByIdAsync(updateEnrollmentViewModel.ClassId);
            updateActualNumberNewClass.ActualNumber = updateActualNumberNewClass.ActualNumber + 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberNewClass);

            var updateActualNumberOldClass = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            updateActualNumberOldClass.ActualNumber = updateActualNumberOldClass.ActualNumber - 1;
            _unitOfWork.ClassRepository.Update(updateActualNumberOldClass);

            //updateScheduleAtten
            //var schedulesUpdate = await _unitOfWork.ScheduleRepository.GetScheduleByClass(updateEnrollmentViewModel.ClassId);
            //updateAttendanceClass
            //var listAttendances = await _unitOfWork.AttendanceRepository.GetListAttendanceByClassIdAndChilIdOutOfStatusFuture(result.ClassId, result.ChildrenProfileId);
            //foreach (var attendance in listAttendances)
            //{
            //    attendance.ScheduleId = schedulesUpdate.FirstOrDefault().Id;
            //    _unitOfWork.AttendanceRepository.Update(attendance);
            //}

            var attendanceCheckTime = await _unitOfWork.AttendanceRepository.GetListAttendanceByClassIdAndChilIdAndOutOfStatusFuture(result.ClassId, result.ChildrenProfileId);           
            result.ClassId = updateEnrollmentViewModel.ClassId;
            var schedules1 = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.ClassId);           
            var classed = await _unitOfWork.ClassRepository.GetByIdAsync(result.ClassId);
            var course = await _unitOfWork.CourseRepository.GetByIdAsync(classed.CourseId);
            DateTime startDate = attendanceCheckTime.Date;

            int slot1 = 0;
            while (slot1 < course.DurationTotal - slot)
            {
                if (schedules1.Any(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())))
                {
                    var attendance = new CreateAttendanceViewModel
                    {
                        ScheduleId = schedules1.FirstOrDefault(x => x.DayInWeek.Contains(startDate.DayOfWeek.ToString())).Id,
                        ChildrenProfileId = result.ChildrenProfileId,
                        Date = startDate,
                        StatusAttendance = StatusAttendance.Future,
                        Note = ""
                    };

                    var attendances = await _unitOfWork.AttendanceRepository.GetListAttendancesByChildId(result.ChildrenProfileId);
                    foreach (var atten in attendances)
                    {
                        if (atten.Date.Date == startDate.Date)
                        {
                            if (schedules1.Any(x => x.SlotId.ToString().Contains(atten.Schedule.SlotId.ToString())))
                            {
                                throw new Exception("Lớp học này có lịch trùng với lịch học bạn đã đăng kí ");
                            }
                        }
                    }
   
                    var attendanceEntity = _mapper.Map<Attendance>(attendance);
                    await _unitOfWork.AttendanceRepository.AddAsync(attendanceEntity);
                    slot1++;
                }

                startDate = startDate.AddDays(1);

                if (slot1 == course.DurationTotal)
                {
                    break; 
                }
            }


            _unitOfWork.EnrollmentRepository.Update(result);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật tham gia này thất bại");
        }

        public async Task<bool> ImportExcelFile(IFormFile formFile)
        {
            //Create a List of Enrollment that Read from Excel File.
            var enrollmetListFromFile = new CreateEnrollmentViewModel();
            var classId = Guid.Empty;

            //ReadExcelFile
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var excelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    List<Guid> listChildrenGuid = new List<Guid>();

                    var findChildren = await _unitOfWork.ChildrenRepository.GetAllAsync();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var col = 1;
                        try
                        {
                            var mssv = worksheet.Cells[row, col].Value.ToString()!.Trim();
                            listChildrenGuid.Add(findChildren.FirstOrDefault(x => x.ChildrenCode.Equals(mssv)).Id);
                        }
                        catch (Exception)
                        {
                            await stream.DisposeAsync();
                            throw new InvalidDataException($"Lỗi tại dòng {row}, Tên cột: {worksheet.Cells[1, col].Value}, Lỗi: Ô này có giá trị trống hoặc giá trị không hợp lệ.");
                        }
                    }
                    try
                    {
                        var classCode = worksheet.Cells[2, 3].Value.ToString().Trim();
                        classId = _unitOfWork.ClassRepository.GetAllAsync().Result.FirstOrDefault(x => x.ClassCode.Equals(classCode)).Id;
                    }
                    catch (Exception)
                    {
                        await stream.DisposeAsync();
                        throw new InvalidDataException($"Tên cột: Mã lớp, Lỗi: Ô này có giá trị trống hoặc giá trị không hợp lệ.");
                    }                 
                    enrollmetListFromFile = new CreateEnrollmentViewModel()
                    {
                        ClassId = classId,
                        ChildrenProfileIds = listChildrenGuid
                    };

                }
            }
            if (enrollmetListFromFile is null) throw new Exception("Thêm trẻ bằng file excel thất bại.");
            await CreateEnrollment(enrollmetListFromFile);
            return true;
        }
    }
}
