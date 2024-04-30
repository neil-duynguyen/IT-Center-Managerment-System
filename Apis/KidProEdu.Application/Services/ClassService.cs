using AutoMapper;
using ClosedXML;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Classes;
using KidProEdu.Application.ViewModels.ChildrenAnswerViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using OfficeOpenXml;
using ZXing;

namespace KidProEdu.Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly IChildrenAnswerService _childrenAnswerService;

        public ClassService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, IChildrenAnswerService childrenAnswerService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _childrenAnswerService = childrenAnswerService;
        }

        public async Task<bool> CreateClass(CreateClassViewModel createClassViewModel)
        {
            var validator = new CreateClassViewModelValidator(_currentTime);
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
            else if (result.StatusOfClass == Domain.Enums.StatusOfClass.Started)
            {
                throw new Exception("Không thể xóa khi lớp đang học");
            }
            else
            {
                var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(result.Id);

                _unitOfWork.ScheduleRepository.SoftRemoveRange(schedules);
                _unitOfWork.ClassRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa lớp thất bại");
            }
        }

        public async Task<ClassViewModel> GetClassById(Guid classId)
        {
            var getClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);

            var mapper = _mapper.Map<ClassViewModel>(getClass);

            if (getClass.StatusOfClass == StatusOfClass.Started)
            {
                List<ScheduleClassViewModel> scheduleClassView = new List<ScheduleClassViewModel>();
                foreach (var item in getClass.Schedules)
                {
                    var getRoom = _unitOfWork.ScheduleRoomRepository.GetAllAsync().Result.Where(x => x.Schedule.Id == item.Id).FirstOrDefault(x => x.Status == ScheduleRoomStatus.Using);
                    var getTeacher = _unitOfWork.TeachingClassHistoryRepository.GetAllAsync().Result.Where(x => x.ClassId == classId).FirstOrDefault(x => x.TeachingStatus == TeachingStatus.Teaching);
                    scheduleClassView.Add(new ScheduleClassViewModel() { Slot = item.Slot.Name, StartTime = item.StartTime, EndTime = item.EndTime, DayInWeek = item.DayInWeek, RoomName = getRoom != null ? getRoom.Room.Name : null, TeacherId = getTeacher != null ? getTeacher.UserAccount.Id : null, TeacherName = getTeacher != null ? getTeacher.UserAccount.FullName : null });
                }
                mapper.scheduleClassViews = scheduleClassView;
            }

            if (getClass.StatusOfClass == StatusOfClass.Expired)
            {
                List<ScheduleClassViewModel> scheduleClassView = new List<ScheduleClassViewModel>();
                foreach (var item in getClass.Schedules)
                {
                    var getRoom = _unitOfWork.ScheduleRoomRepository.GetAllAsync().Result.Where(x => x.Schedule.Id == item.Id).FirstOrDefault(x => x.Status == ScheduleRoomStatus.Expired);
                    var getTeacher = _unitOfWork.TeachingClassHistoryRepository.GetAllAsync().Result.Where(x => x.ClassId == classId).FirstOrDefault(x => x.TeachingStatus == TeachingStatus.Leaved);
                    scheduleClassView.Add(new ScheduleClassViewModel() { Slot = item.Slot.Name, StartTime = item.StartTime, EndTime = item.EndTime, DayInWeek = item.DayInWeek, RoomName = getRoom != null ? getRoom.Room.Name : null, TeacherId = getTeacher != null ? getTeacher.UserAccount.Id : null, TeacherName = getTeacher != null ? getTeacher.UserAccount.FullName : null });
                }
                mapper.scheduleClassViews = scheduleClassView;
            }

            if (getClass.StatusOfClass == StatusOfClass.Pending)
            {
                List<ScheduleClassViewModel> scheduleClassView = new List<ScheduleClassViewModel>();
                foreach (var item in getClass.Schedules)
                {
                    var getRoom = _unitOfWork.ScheduleRoomRepository.GetAllAsync().Result.Where(x => x.Schedule.Id == item.Id).FirstOrDefault(x => x.Status == ScheduleRoomStatus.Pending);
                    var getTeacher = _unitOfWork.TeachingClassHistoryRepository.GetAllAsync().Result.Where(x => x.ClassId == classId).FirstOrDefault(x => x.TeachingStatus == TeachingStatus.Pending);
                    scheduleClassView.Add(new ScheduleClassViewModel() { Slot = item.Slot.Name, StartTime = item.StartTime, EndTime = item.EndTime, DayInWeek = item.DayInWeek, RoomName = getRoom != null ? getRoom.Room.Name : null, TeacherId = getTeacher != null ? getTeacher.UserAccount.Id : null, TeacherName = getTeacher != null ? getTeacher.UserAccount.FullName : null });
                }
                mapper.scheduleClassViews = scheduleClassView;
            }

            return mapper;
        }

        public async Task<List<ClassViewModel>> GetClasses()
        {
            var listClass = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            /*List<ClassViewModel> classViewModels = new List<ClassViewModel>();

            foreach (var classDetail in listClass)
            {
                List<ScheduleClassViewModel> scheduleClassView = new List<ScheduleClassViewModel>();
                foreach (var item in classDetail.Schedules)
                {
                    scheduleClassView.Add(new ScheduleClassViewModel() { Slot = item.Slot.Name, StartTime = item.StartTime, EndTime = item.EndTime, DayInWeek = item.DayInWeek });
                }
                var mapper = _mapper.Map<ClassViewModel>(classDetail);
                mapper.scheduleClassViews = scheduleClassView;

                classViewModels.Add(mapper);
            }*/
            var mapper = _mapper.Map<List<ClassViewModel>>(listClass);

            return mapper;
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

        //api này dùng để làm api start/end/cancel class luôn
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
                    // chỉ pending mới được start
                    // start class ghi nhận teachingHistory
                    // ghi nhận trạng thái scheduleRoom
                    // ghi nhận trạng thái phòng học
                    {

                        foreach (var item in changeStatusClassViewModel.ids)
                        {
                            var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(item);

                            if (findClass.StatusOfClass == Domain.Enums.StatusOfClass.Pending)
                            {
                                // ghi nhận teachingHistory
                                var teachingHistory = _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByClassId(item)
                                    .Result.FirstOrDefault(x => x.TeachingStatus == Domain.Enums.TeachingStatus.Pending);
                                teachingHistory.TeachingStatus = Domain.Enums.TeachingStatus.Teaching;

                                _unitOfWork.TeachingClassHistoryRepository.Update(teachingHistory);

                                // bắt đầu ghi nhận trạng thái room và scheduleRoom
                                var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(item);
                                foreach (var schedule in schedules)
                                {
                                    // ghi nhận trạng thái phòng học
                                    var listScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                        .Result.Where(x => x.Status == Domain.Enums.ScheduleRoomStatus.Pending).ToList();
                                    /*var listScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                        .Result.Where(x => x.Status == Domain.Enums.ScheduleRoomStatus.Using
                                        || x.Status == Domain.Enums.ScheduleRoomStatus.Pending)
                                        .DistinctBy(x => x.RoomId).ToList();*/
                                    foreach (var scheduleRoom in listScheduleRoom)
                                    {
                                        var room = await _unitOfWork.RoomRepository.GetByIdAsync((Guid)scheduleRoom.RoomId);
                                        room.Status = Domain.Enums.StatusOfRoom.Used;

                                        _unitOfWork.RoomRepository.Update(room);
                                    }

                                    // ghi nhận scheduleRoom
                                    var findScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                        .Result.FirstOrDefault(x => x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Pending));

                                    findScheduleRoom.Status = Domain.Enums.ScheduleRoomStatus.Using;

                                    _unitOfWork.ScheduleRoomRepository.Update(findScheduleRoom);
                                }

                                // ghi nhận trạng thái lớp học
                                /*// change status bỏ qua những class đã cancel hoặc expired
                                if (findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Cancel)
                                    || findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Expired))
                                    continue;*/

                                findClass.StatusOfClass = status;

                                _unitOfWork.ClassRepository.Update(findClass);

                            }
                            else
                            {
                                listChildrenPassed.Add(new ChildrenPassedViewModel()
                                {
                                    Class = _mapper.Map<ClassViewModel>(findClass)
                                });
                            }

                        }
                    }

                    break;
                case Domain.Enums.StatusOfClass.Cancel:
                    // chỉ lớp pending mới được cancel
                    // hủy lớp học
                    // hủy thì chỉ đổi status lớp học thôi, còn lịch và phòng thì vẫn pending và có thể xếp lại
                    // gửi thông báo đến cho staff để xử lí nghiệp vụ tư vấn lớp mới
                    // xóa attendance của thằng children đó và xóa enrollment của nó khỏi lớp
                    // nếu muốn học lại thực hiện đăng kí mới (staff add enrollment mới)
                    {

                        foreach (var classId in changeStatusClassViewModel.ids)
                        {
                            var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);

                            if (findClass.StatusOfClass == Domain.Enums.StatusOfClass.Pending)
                            {
                                // lấy list học sinh theo lớp
                                var childrenInClass = _unitOfWork.EnrollmentRepository.GetAllAsync().Result.Where(x => x.ClassId == classId).ToList();

                                // lấy ra những staff mà add children đó vô lớp bằng enrollment
                                var listStaffForChildren = childrenInClass.DistinctBy(x => x.UserId);
                                // gửi mail cho staff chịu trách nhiệm quản lý
                                foreach (var enrollment in listStaffForChildren)
                                {
                                    // lấy lại list children theo staff
                                    var listChildren = _unitOfWork.EnrollmentRepository.GetAllAsync().Result
                                        .Where(x => x.UserId == enrollment.UserId).ToList();

                                    // tạo file excel chứa list children theo staff
                                    var file = await ExportExcelFileByListAsync(listChildren, "Danh_sach_hs.xlsx");

                                    // lấy staff ra để lấy email
                                    var staff = await _unitOfWork.UserRepository.GetByIdAsync(enrollment.UserId);

                                    // gửi mail cho từng staff kèm theo file excel list hs mà staff đó add vô (phụ trách) đính kèm

                                    await SendEmailUtil.SendEmailWithAttachment(staff.Email, "Thông báo về việc lớp học bị hủy",
                                        "<p>Thông báo đến thầy/cô phụ trách học sinh, </p></br>" +
                                        "<p>Hiện lớp " + findClass.ClassCode + " thuộc môn " + findClass.Course.Name +
                                        " đã bị hủy do không đủ điều kiện để mở lớp, </p></br>" +
                                        "<p>Thông tin chi tiết được gửi trong file đính kèm, quý thầy cô thực hiện tư vấn lại " +
                                        "với phụ huynh học sinh để tiến hành đăng kí mới nếu có nguyện vọng! </p></br>" +
                                        "<p>Trân trọng, </p></br>" +
                                        "<p>KidPro Education!</p>"
                                        , file);
                                }

                                // gửi mail cho parents 
                                foreach (var children in childrenInClass)
                                {
                                    var childrenProfile = await _unitOfWork.ChildrenRepository.GetByIdAsync(children.ChildrenProfileId);

                                    await SendEmailUtil.SendEmail(childrenProfile.UserAccount.Email, "Thông báo về việc lớp học bị hủy",
                                        "Thông báo đến quý phụ huynh học sinh, \n\n" +
                                        "Hiện lớp " + findClass.ClassCode + " thuộc môn " + findClass.Course.Name +
                                        " đã bị hủy do không đủ điều kiện để mở lớp, vui lòng liên hệ nhân viên" +
                                        " để được hỗ trợ tư vấn " +
                                        "đăng kí lớp học mới cho học sinh trong vòng 1 tuần kể từ lúc nhận mail \n\n" +
                                        //"Nhân viên của chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất. \n\n" +
                                        "Trân trọng, \n" +
                                        "KidPro Education!");

                                    // xóa attendance của thằng đó trước
                                    foreach (var schedule in findClass.Schedules)
                                    {
                                        var listAttendance = await _unitOfWork.AttendanceRepository.GetListAttendanceByScheduleIdAndChilId(schedule.Id, children.ChildrenProfileId);
                                        _unitOfWork.AttendanceRepository.RemoveRange(listAttendance);
                                    }
                                }

                                _unitOfWork.EnrollmentRepository.RemoveRange(childrenInClass);

                                // ghi nhận trạng thái lớp học

                                /*// change status bỏ qua những class đã cancel hoặc expired
                                if (findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Cancel)
                                    || findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Expired))
                                    continue;*/

                                findClass.StatusOfClass = status;

                                _unitOfWork.ClassRepository.Update(findClass);
                            }
                            else
                            {
                                listChildrenPassed.Add(new ChildrenPassedViewModel()
                                {
                                    Class = _mapper.Map<ClassViewModel>(findClass)
                                });
                            }
                        }
                    }

                    break;
                case Domain.Enums.StatusOfClass.Expired:
                    // chỉ lớp start mới end
                    // end class
                    // ghi nhận trạng thái teachingHistory
                    // ghi nhận trạng thái scheduleRoom
                    // check điểm bài kiểm tra và cấp chứng chỉ
                    {

                        foreach (var classId in changeStatusClassViewModel.ids) // lấy từng id trong list class Id
                        {
                            var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);

                            if (findClass.StatusOfClass == Domain.Enums.StatusOfClass.Started)
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

                                // bắt đầu ghi nhận trạng thái room và scheduleRoom
                                var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(classId);
                                foreach (var schedule in schedules)
                                {
                                    // ghi nhận trạng thái room
                                    var listScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                        .Result.Where(x => x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Using)).ToList();

                                    /*var listScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                        .Result.Where(x => x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Using)).DistinctBy(x => x.RoomId).ToList();*/

                                    foreach (var scheduleRoom in listScheduleRoom)
                                    {
                                        var room = await _unitOfWork.RoomRepository.GetByIdAsync((Guid)scheduleRoom.RoomId);
                                        room.Status = Domain.Enums.StatusOfRoom.Empty;

                                        _unitOfWork.RoomRepository.Update(room);
                                    }

                                    // ghi nhận trạng thái schedule room
                                    var scheduleRooms = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                                        .Result.Where(x => x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Using)
                                        || x.Status.Equals(Domain.Enums.ScheduleRoomStatus.Temp)).ToList();
                                    foreach (var scheduleRoom in scheduleRooms)
                                    {
                                        scheduleRoom.Status = Domain.Enums.ScheduleRoomStatus.Expired;
                                        _unitOfWork.ScheduleRoomRepository.Update(scheduleRoom);
                                    }
                                }

                                // check điểm bài ktra để cấp chứng chỉ
                                var childrensInClass = await GetChildrenByClassId(classId);
                                findClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
                                var exams = await _unitOfWork.ExamRepository.GetExamByCourseId(findClass.CourseId);

                                foreach (var children in childrensInClass) // duyệt từng children trong class để check pass/not pass => cấp chứng chỉ
                                {
                                    var childrenProfile = await _unitOfWork.ChildrenRepository.GetByIdAsync(children.ChildrenProfileId);

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
                                                    .Where(x => x.ChildrenProfileId == childrenProfile.Id
                                                    && x.ExamId == exam.Id).ToList();
                                                foreach (var question in pt)
                                                {
                                                    ptPoint += question.ScorePerQuestion;
                                                }
                                                break;
                                            case Domain.Enums.TestType.MidTerm:
                                                var midterm = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                                                    .Where(x => x.ChildrenProfileId == childrenProfile.Id
                                                    && x.ExamId == exam.Id).ToList();
                                                foreach (var question in midterm)
                                                {
                                                    midTermPoint += question.ScorePerQuestion;
                                                }
                                                break;
                                            case Domain.Enums.TestType.Final:
                                                var final = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result
                                                    .Where(x => x.ChildrenProfileId == childrenProfile.Id
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
                                    var totalScore = ptPoint * (await _unitOfWork.ConfigPointMultiplierRepository.GetConfigPointByTestType(TestType.Progress)).Multiplier
                                        + midTermPoint * (await _unitOfWork.ConfigPointMultiplierRepository.GetConfigPointByTestType(TestType.MidTerm)).Multiplier
                                        + finalPoint * (await _unitOfWork.ConfigPointMultiplierRepository.GetConfigPointByTestType(TestType.Final)).Multiplier;

                                    /*// lấy tình trạng điểm danh của children trong course đó
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
                                    }*/

                                    // trả ra list children để cấp certificate nếu đủ điều kiện pass (trung bình >=5, k nghỉ quá 20%)
                                    if (totalScore >= 0) // mốt phải sửa lại là >=5, đang test
                                    {
                                        listChildrenPassed.Add(new ChildrenPassedViewModel()
                                        {
                                            ChildrenProfile = _mapper.Map<ChildrenProfileViewModel>(childrenProfile),
                                            Course = _mapper.Map<CourseViewModel>(await _unitOfWork.CourseRepository.GetByIdAsync(findClass.CourseId))
                                        });

                                    }
                                    /*if (totalScore >= 5 && (listAbsent.Count <= (listAttendances.Count * 0.2)))
                                    {
                                        listChildrenPassed.Add(new ChildrenPassedViewModel()
                                        {
                                            ChildrenProfile = _mapper.Map<ChildrenProfileViewModel>(children),
                                            Course = _mapper.Map<CourseViewModel>(await _unitOfWork.CourseRepository.GetByIdAsync(findClass.CourseId))
                                        });

                                    }*/

                                }

                                // ghi nhận trạng thái lớp học
                                /*// change status bỏ qua những class đã cancel hoặc expired
                                if (findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Cancel)
                                    || findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Expired))
                                    continue;*/

                                findClass.StatusOfClass = status;

                                _unitOfWork.ClassRepository.Update(findClass);
                            }
                            else
                            {
                                listChildrenPassed.Add(new ChildrenPassedViewModel()
                                {
                                    Class = _mapper.Map<ClassViewModel>(findClass)
                                });
                            }
                        }
                    }

                    break;
                default:
                    break;
            }

            /*// ghi nhận trạng thái lớp học
            foreach (var item in changeStatusClassViewModel.ids)
            {
                var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(item);

                // change status bỏ qua những class đã cancel hoặc expired
                if (findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Cancel)
                    || findClass.StatusOfClass.Equals(Domain.Enums.StatusOfClass.Expired))
                    continue;

                findClass.StatusOfClass = status;

                _unitOfWork.ClassRepository.Update(findClass);
            }*/

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
            string[] columnNames = new string[] { "ChildrenCode", "FullName","ExamCode", "ScorePerQuestion" };

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

        //function này dùng để nhâp điểm bằng file excel
        public async Task<bool> ImportScoreExcelFileAsync(IFormFile formFile)
        {
            //Create a List of ChildrenAnswer that Read from Excel File.
            var childrenAnswerListFromFile = new List<CreateChildrenAnswerViewModel>();
            Question question = new Question() { Id = Guid.NewGuid(), Title = "Bài thi thực hành", Type = QuestionType.Course};
            await _unitOfWork.QuestionRepository.AddAsync(question);
            
            //ReadExcelFile
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var excelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var findChildren = await _unitOfWork.ChildrenRepository.GetAllAsync();
                    var findExam = await _unitOfWork.ExamRepository.GetAllAsync();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var col = 1;
                        try 
                        {
                            var mssv = worksheet.Cells[row, col++].Value.ToString()!.Trim();
                            var examCode = worksheet.Cells[row, ++col].Value.ToString()!.Trim();
                            var scorePerQuestion = worksheet.Cells[row, ++col].Value;
                            var foundExam = findExam.FirstOrDefault(x => x.TestCode.Equals(examCode)) ?? throw new Exception($"Không tìm thấy bài kiểm tra với mã {examCode}.");

                            childrenAnswerListFromFile.Add(new CreateChildrenAnswerViewModel() {ChildrenProfileId = findChildren.FirstOrDefault(x => x.ChildrenCode.Equals(mssv)).Id, 
                                                                                                QuestionId = question.Id,
                                                                                                ExamId = foundExam.Id,
                                                                                                ScorePerQuestion = (double)scorePerQuestion });
                        }
                        catch (InvalidDataException)
                        {
                            await stream.DisposeAsync();
                            throw new InvalidDataException($"Lỗi tại dòng {row}, Tên cột: {worksheet.Cells[1, col].Value}, Lỗi: Ô này có giá trị trống hoặc giá trị không hợp lệ.");
                        }
                        catch (Exception ex)
                        {
                            await stream.DisposeAsync();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }

            await _unitOfWork.SaveChangeAsync();
            if (childrenAnswerListFromFile.Count == 0) throw new Exception("Nhập điểm bằng file excel thất bại.");
            return await _childrenAnswerService.CreateChildrenAnswers(childrenAnswerListFromFile);
        }

        public async Task<MimePart> ExportExcelFileByListAsync(List<Enrollment> list, string attachmentFileName)
        {
            string[] columnNames = new string[] { "ChildrenCode", "FullName", "Class", "Parents", "Phone", "Gmail" };

            var memoryStream = new MemoryStream();
            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Danh sách học sinh");

            // Add header
            for (int i = 0; i < columnNames.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = columnNames[i];
            }

            /*var getClassById = _unitOfWork.ClassRepository.GetAllAsync().Result.FirstOrDefault(x => x.Id == classId);
            var getListChildren = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByClassId(getClassById.Id);*/

            // Add data
            for (int i = 0; i < list.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = list[i].ChildrenProfile.ChildrenCode;
                worksheet.Cell(i + 2, 2).Value = list[i].ChildrenProfile.FullName;
                worksheet.Cell(i + 2, 3).Value = list[i].Class.ClassCode;
                worksheet.Cell(i + 2, 4).Value = list[i].ChildrenProfile.UserAccount.FullName;
                worksheet.Cell(i + 2, 5).Value = list[i].ChildrenProfile.UserAccount.Phone;
                worksheet.Cell(i + 2, 6).Value = list[i].ChildrenProfile.UserAccount.Email;
            }

            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Tạo attachment từ MemoryStream
            var attachment = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                Content = new MimeContent(memoryStream),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachmentFileName
            };


            /* stream.Position = 0;*/

            return attachment;
        }

        public async Task<bool> TestSendAttachEmail()
        {
            var list = await _unitOfWork.EnrollmentRepository.GetEnrollmentsByClassId(new Guid("2e741406-fccc-4ba9-8c50-08dc53038619"));
            var file = await ExportExcelFileByListAsync(list, "danh_sach_hs.xlsx");

            await SendEmailUtil.SendEmailWithAttachment("tkchoi1312@gmail.com", "Thông báo về việc hủy bỏ lớp không đủ điều kiện bắt đầu",
                                "<p>Thông báo đến thầy/cô phụ trách học sinh, </p></br>" +
                                //"<p>Hiện lớp " + findClass.ClassCode + " thuộc môn " + findClass.Course.Name +
                                //" đã bị hủy do không đủ điều kiện để mở lớp, </p></br>" +
                                "<p>Thông tin chi tiết được gửi trong file đính kèm, quý thầy cô thực hiện tư vấn lại " +
                                "với phụ huynh học sinh để tiến hành đăng kí mới nếu có nguyện vọng! </p></br>" +
                                "<p>Trân trọng, </p></br>" +
                                "<p>KidPro Education!</p>"
                                , file);

            return true;
        }

        public async Task<bool> ChangeTeacherForClass(ChangeTeacherForClassViewModel changeTeacherForClassViewModel)
        {
            var teachingHistory = _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByClassId(changeTeacherForClassViewModel.ClassId)
                .Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.TeachingStatus == Domain.Enums.TeachingStatus.Teaching
                || x.TeachingStatus == Domain.Enums.TeachingStatus.Pending);

            var findClass = await _unitOfWork.ClassRepository.GetByIdAsync(changeTeacherForClassViewModel.ClassId);

            for (var i = 0; i < 7; i++)
            {
                var getDate = changeTeacherForClassViewModel.StartDate.AddDays(i);
                var schedule = findClass.Schedules.FirstOrDefault(x => x.DayInWeek == getDate.DayOfWeek.ToString());

                if (schedule != null)
                {
                    var otherSchedule = findClass.Schedules.FirstOrDefault(x => x.Id != schedule.Id);

                    var newTeachingHistory = new TeachingClassHistory()
                    {
                        UserAccountId = changeTeacherForClassViewModel.TeacherId,
                        ClassId = changeTeacherForClassViewModel.ClassId,
                        StartDate = getDate,
                        TeachingStatus = Domain.Enums.TeachingStatus.Teaching
                    };

                    await _unitOfWork.TeachingClassHistoryRepository.AddAsync(newTeachingHistory);

                    for (var j = 1; i < 6 - i; j++)
                    {
                        var getOtherDate = getDate.AddDays(j);
                        if (getOtherDate.DayOfWeek.ToString() == otherSchedule.DayInWeek)
                        {
                            teachingHistory.EndDate = getOtherDate.AddDays(-7);

                            _unitOfWork.TeachingClassHistoryRepository.Update(teachingHistory);

                            break;
                        }
                    }

                    break;
                }
            }

            /*if (changeTeacherForClassViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("tuesday")
                || changeTeacherForClassViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("wednesday")
                || changeTeacherForClassViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("thursday"))
            {
                teachingHistory.EndDate = changeTeacherForClassViewModel.StartDate.AddDays(-4);
            }
            else
            {
                teachingHistory.EndDate = changeTeacherForClassViewModel.StartDate.AddDays(-3);
            }

            var newTeachingHistory = new TeachingClassHistory()
            {
                UserAccountId = changeTeacherForClassViewModel.TeacherId,
                ClassId = changeTeacherForClassViewModel.ClassId,
                StartDate = changeTeacherForClassViewModel.StartDate,
                TeachingStatus = Domain.Enums.TeachingStatus.Teaching
            };


            _unitOfWork.TeachingClassHistoryRepository.Update(teachingHistory);
            await _unitOfWork.TeachingClassHistoryRepository.AddAsync(newTeachingHistory);*/

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Đổi giáo viên thất bại");
        }

        //api get list class teacher is teaching
        public async Task<List<ClassViewModel>> GetListClassTeachingByTeacher(Guid teacherId)
        {
            var listTeachingClassHistory = _unitOfWork.TeachingClassHistoryRepository.GetAllAsync().Result.Where(x => x.UserAccountId == teacherId && x.TeachingStatus == TeachingStatus.Pending).ToList();

            List<ClassViewModel> listClassViewModels = new List<ClassViewModel>();

            foreach (var item in listTeachingClassHistory)
            {
                var classView = await GetClassById(item.ClassId);
                listClassViewModels.Add(classView);
            }

            return listClassViewModels;
        }

        //api này lấy ra lớp có status pending nhưng đã xếp gv và room rồi
        public async Task<List<ClassViewModel>> GetListClassStatusPending()
        {
            var getClassPending = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.StatusOfClass == StatusOfClass.Pending).ToList();

            List<ClassViewModel> listClassViewModels = new List<ClassViewModel>();

            foreach (var item in getClassPending)
            {
                var classView = await GetClassById(item.Id);
                listClassViewModels.Add(classView);
            }

            listClassViewModels.RemoveAll(item => item.scheduleClassViews.Any(x => x.RoomName == null && x.TeacherName == null));

            return listClassViewModels;
        }
    }
}
