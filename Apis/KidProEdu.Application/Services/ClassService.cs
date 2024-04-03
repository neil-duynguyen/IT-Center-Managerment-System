using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Classes;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Linq.Expressions;

namespace KidProEdu.Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ClassService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateClass(CreateClassViewModel createClassViewModel)
        {
            var validator = new CreateClassViewModelValidator();
            var validationResult = validator.Validate(createClassViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var findClass = await _unitOfWork.ClassRepository.GetClassByClassCode(createClassViewModel.ClassCode);
            if (!findClass.IsNullOrEmpty())
            {
                throw new Exception("Lớp đã tồn tại");
            }

            var mapper = _mapper.Map<Class>(createClassViewModel);

            mapper.StatusOfClass = Domain.Enums.StatusOfClass.Pending;
            mapper.ActualNumber = 0;

            await _unitOfWork.ClassRepository.AddAsync(mapper);

            ScheduleService sv = new ScheduleService(_unitOfWork, _currentTime, _claimsService, _mapper);
            foreach (var item in createClassViewModel.createScheduleViewModel)
            {
                await sv.CreateSchedule(item, mapper.Id);
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo lớp thất bại");

        }

        public async Task<bool> DeleteClass(Guid ClassId)
        {
            var result = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);

            if (result == null)
                throw new Exception("Không tìm thấy lớp này");
            else
            {
                _unitOfWork.ClassRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa lớp thất bại");
            }
        }

        public async Task<ClassViewModel> GetClassById(Guid ClassId)
        {
            var getClass = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            return _mapper.Map<ClassViewModel>(getClass); ;
        }

        public async Task<List<ClassViewModel>> GetClasses()
        {
            var Classs = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<ClassViewModel>>(Classs);
        }

        public async Task<bool> UpdateClass(UpdateClassViewModel updateClassViewModel)
        {
            var validator = new UpdateClassViewModelValidator();
            var validationResult = validator.Validate(updateClassViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(updateClassViewModel.Id)
                ?? throw new Exception("Không tìm thấy lớp");

            if (updateClassViewModel.MaxNumber < findClass.ActualNumber)
            {
                throw new Exception("Số lượng học sinh tối đa không thể nhỏ hơn số lượng học sinh thực tế đang có trong lớp");
            }
            /*else if (updateClassViewModel.MaxNumber < findClass.ExpectedNumber)
            {
                throw new Exception("Số lượng học sinh dự kiến không thể lớn hơn số lượng học tối đa của lớp");
            }*/

            var existingClass = await _unitOfWork.ClassRepository.GetClassByClassCode(updateClassViewModel.ClassCode);
            if (!existingClass.IsNullOrEmpty())
            {
                foreach (var item in existingClass)
                {
                    if (item.Id != updateClassViewModel.Id)
                    {
                        throw new Exception("Lớp đã tồn tại");
                    }
                }
            }

            if (findClass.StatusOfClass != Domain.Enums.StatusOfClass.Pending)
                throw new Exception("Cập nhật lớp thất bại, lớp này hiện không còn trong trạng thái chờ");

            /*Class.ClassName = updateClassViewModel.ClassName;
            Class.Description = updateClassViewModel.Description;*/

            var mapper = _mapper.Map(updateClassViewModel, findClass);

            _unitOfWork.ClassRepository.Update(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật lớp thất bại");
        }

        //api này dùng để làm api start/end class luôn
        public async Task<List<ChildrenPassedViewModel>> ChangeStatusClass(ChangeStatusClassViewModel changeStatusClassViewModel)
        {
            Domain.Enums.StatusOfClass status = changeStatusClassViewModel.status switch
            {
                "Started" => Domain.Enums.StatusOfClass.Started,
                //"Pending" => Domain.Enums.StatusOfClass.Pending,
                "Cancel" => Domain.Enums.StatusOfClass.Cancel,
                "Expired" => Domain.Enums.StatusOfClass.Expired,
                _ => throw new Exception("Trạng thái không được hỗ trợ"),
            };

            var listChildrenPassed = new List<ChildrenPassedViewModel>();

            switch (status)
            {
                /*case Domain.Enums.StatusOfClass.Pending:
                    break;*/
                case Domain.Enums.StatusOfClass.Started:
                    // start class ghi nhận teachingHistory
                    // ghi nhận trạng thái phòng học

                    foreach (var item in changeStatusClassViewModel.ids)
                    {

                        // ghi nhận teachingHistory
                        var teachingHistory = _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByClassId(item)
                            .Result.FirstOrDefault(x => x.TeachingStatus == Domain.Enums.TeachingStatus.Pending);
                        teachingHistory.TeachingStatus = Domain.Enums.TeachingStatus.Teaching;

                        _unitOfWork.TeachingClassHistoryRepository.Update(teachingHistory);

                        // ghi nhận phòng học
                        var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(item);
                        foreach (var schedule in schedules)
                        {
                            var findScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                .Result.FirstOrDefault(x => x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Pending));
                            findScheduleRoom.Status = Domain.Enums.ScheduleRoomStatus.Using;

                            _unitOfWork.ScheduleRoomRepository.Update(findScheduleRoom);
                        }
                    }

                    break;
                case Domain.Enums.StatusOfClass.Cancel:
                    // hủy lớp học
                    // hủy thì chỉ đổi status lớp học thôi, còn lịch và phòng thì vẫn pending và có thể xếp lại
                    // gửi thông báo đến cho staff để xử lí nghiệp vụ chuyển lớp hay gì đó

                    foreach (var classId in changeStatusClassViewModel.ids)
                    {
                        var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);

                        // lấy list học sinh theo lớp
                        var childrenInClass = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.ClassId == classId).ToList();

                        // lấy ra những staff mà add children đó vô lớp bằng enrollment
                        var listStaffForChildren = childrenInClass.DistinctBy(x => x.UserId);
                        foreach (var enrollment in listStaffForChildren)
                        {
                            var staff = await _unitOfWork.UserRepository.GetByIdAsync(enrollment.UserId);
                            await SendEmailUtil.SendEmail(staff.Email, "Thông báo về việc lớp học bị hủy",
                                "Thông báo đến thầy/cô phụ trách học sinh, \n\n" +
                                "Hiện lớp " + findClass.ClassCode + " thuộc môn " + findClass.Course.Name +
                                " đã bị hủy do không đủ điều kiện để mở lớp, \n" +
                                /*"Thông tin:, \n" +
                                "         Người đăng kí: " + createAdviseRequestViewModel.FullName + "\n" +
                                "         Email: " + createAdviseRequestViewModel.Email + "\n" +
                                "         Sđt: " + createAdviseRequestViewModel.Phone + "\n" +
                                "Nhân viên của chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất. \n\n" +*/
                                "Trân trọng, \n" +
                                "KidPro Education!");
                        }

                    }

                    break;
                case Domain.Enums.StatusOfClass.Expired:
                    // end class
                    // ghi nhận trạng thái teachingHistory
                    // ghi nhận trạng thái scheduleRoom
                    // check điểm bài kiểm tra và cấp chứng chỉ

                    foreach (var classId in changeStatusClassViewModel.ids) // lấy từng id trong list class Id
                    {
                        // ghi nhận teachingHistory
                        var teachingHistories = _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByClassId(classId)
                            .Result.Where(x => x.TeachingStatus == Domain.Enums.TeachingStatus.Teaching
                             || x.TeachingStatus == Domain.Enums.TeachingStatus.Substitute).ToList();
                        foreach (var teachingHistory in teachingHistories)
                        {
                            teachingHistory.TeachingStatus = Domain.Enums.TeachingStatus.Leaved;
                            _unitOfWork.TeachingClassHistoryRepository.Update(teachingHistory);
                        }

                        // ghi nhận phòng học
                        var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(classId);
                        foreach (var schedule in schedules)
                        {
                            var scheduleRooms = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                .Result.Where(x => x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Using)
                                || x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Temp));
                            foreach (var scheduleRoom in scheduleRooms)
                            {
                                scheduleRoom.Status = Domain.Enums.ScheduleRoomStatus.Using;
                                _unitOfWork.ScheduleRoomRepository.Update(scheduleRoom);
                            }
                        }

                        // check điểm bài ktra và cấp chứng chỉ
                        var childrensInClass = await GetChildrenByClassId(classId);
                        var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
                        var exams = await _unitOfWork.ExamRepository.GetExamByCourseId(findClass.CourseId);

                        foreach (var children in childrensInClass) // duyệt từng children trong class để check pass/not pass => cấp chứng chỉ
                        {
                            // lấy tổng trung bình điểm của children trong course đó
                            double ptPoint = 0;
                            //double pt2Point = 0;
                            double midTermPoint = 0;
                            double finalPoint = 0;
                            foreach (var exam in exams) // duyệt từng bài test theo course mà children đó học
                            {
                                switch (exam.TestType)
                                {
                                    case Domain.Enums.TestType.Progress:
                                        var pt = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                                            .Where(x => x.ChildrenProfileId == children.Children.Id
                                            && x.ExamId == exam.Id).ToList();
                                        foreach (var question in pt)
                                        {
                                            ptPoint += question.ScorePerQuestion;
                                        }
                                        break;
                                    case Domain.Enums.TestType.MidTerm:
                                        var midterm = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                                            .Where(x => x.ChildrenProfileId == children.Children.Id
                                            && x.ExamId == exam.Id).ToList();
                                        foreach (var question in midterm)
                                        {
                                            midTermPoint += question.ScorePerQuestion;
                                        }
                                        break;
                                    case Domain.Enums.TestType.Final:
                                        var final = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                                            .Where(x => x.ChildrenProfileId == children.Children.Id
                                            && x.ExamId == exam.Id).ToList();
                                        foreach (var question in final)
                                        {
                                            finalPoint += question.ScorePerQuestion;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            // 2 bài pt mỗi bài 15%, 1 bài midterm 30%, 1 bài final 40%
                            var totalScore = ptPoint * 0.15 + midTermPoint * 0.3 + finalPoint * 0.4;

                            // lấy tình trạng điểm danh của children trong course đó
                            var listAttendances = new List<Attendance>();
                            foreach (var schedule in findClass.Schedules)
                            {
                                listAttendances.AddRange(await _unitOfWork.AttendanceRepository
                                    .GetListAttendanceByScheduleIdAndChilId(schedule.Id, children.Id));
                            }

                            var listAbsent = new List<Attendance>();
                            if (listAttendances.Count > 0)
                            {
                                listAbsent = listAttendances.Where(x => x.StatusAttendance == Domain.Enums.StatusAttendance.Absent).ToList();
                            }

                            // trả ra list children để cấp certificate nếu đủ điều kiện pass (trung bình >=5, k nghỉ quá 20%)
                            if (totalScore >= 5 && (listAbsent.Count <= (listAttendances.Count * 0.2)))
                            {
                                listChildrenPassed.Add(new ChildrenPassedViewModel()
                                {
                                    ChildrenProfile = _mapper.Map<ChildrenProfileViewModel>(children),
                                    Course = _mapper.Map<CourseViewModel>(await _unitOfWork.CourseRepository.GetByIdAsync(findClass.CourseId))
                                });

                            }

                        }

                    }

                    break;
                default:
                    break;
            }

            // ghi nhận trạng thái lớp học
            foreach (var item in changeStatusClassViewModel.ids)
            {
                var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(item);

                // change status bỏ qua những class đã cancel hoặc expired
                if (findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Cancel)
                    || findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Expired))
                    continue;

                findClass.StatusOfClass = status;

                _unitOfWork.ClassRepository.Update(findClass);
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? listChildrenPassed : throw new Exception("Cập nhật trạng thái lớp thất bại");
        }

        //api này dùng để get childen trong classid
        public async Task<List<ClassChildrenViewModel>> GetChildrenByClassId(Guid classId)
        {
            var result = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.ClassId == classId).ToList();
            var mapper = _mapper.Map<List<ClassChildrenViewModel>>(result);
            return mapper;
        }

        public async Task<Stream> ExportExcelFileAsync(Guid classId)
        {
            string[] columnNames = new string[] { "ChildrenCode", "FullName", "ScorePerQuestion" };

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Nhập điểm");

                // Add header
                for (int i = 0; i < columnNames.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnNames[i];
                }

                var getClassById = _unitOfWork.ClassRepository.GetAllAsync().Result.FirstOrDefault(x => x.Id == classId);
                var getListChildren = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByClassId(getClassById.Id);

                // Add data
                for (int i = 0; i < getListChildren.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = getListChildren[i].ChildrenProfile.ChildrenCode;
                    worksheet.Cells[i + 2, 2].Value = getListChildren[i].ChildrenProfile.FullName;
                }

                await package.SaveAsync();
            }

            stream.Position = 0;

            return stream;

        }
    }
}
