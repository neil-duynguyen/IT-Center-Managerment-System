using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Schedules;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
using KidProEdu.Application.ViewModels.SlotViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }
        public async Task<bool> CreateSchedule(CreateScheduleViewModel createScheduleViewModel, Guid classId)
        {
            var validator = new CreateScheduleViewModelValidator();
            var validationResult = validator.Validate(createScheduleViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            /*var schedule = await _unitOfWork.ScheduleRepository.GetScheduleByClass(createScheduleViewModel.ClassId);
            if (!schedule.IsNullOrEmpty())
            {
                throw new Exception("Lịch đã tồn tại");
            }*/

            var mapper = _mapper.Map<Schedule>(createScheduleViewModel);
            mapper.ClassId = classId;
            // Thêm danh sách Attendance vào unitOfWork           
            await _unitOfWork.ScheduleRepository.AddAsync(mapper);
            /*var attendanceList = getListEnrollment.Select(enrollment => new CreateAttendanceViewModel
            {
                ScheduleId = mapper.Id,
                ChildrenProfileId = enrollment.ChildrenProfileId,
                Date = (DateTime)createScheduleViewModel.StartTime,
                StatusAttendance = StatusAttendance.Future,
                Note = ""
            }).ToList();
            var mapper2 = _mapper.Map<List<Attendance>>(attendanceList);
            await _unitOfWork.AttendanceRepository.AddRangeAsync(mapper2);*/
            return true;
        }

        public async Task<bool> DeleteSchedule(Guid ScheduleId)
        {
            var result = await _unitOfWork.ScheduleRepository.GetByIdAsync(ScheduleId);

            if (result == null)
                throw new Exception("Không tìm thấy lịch này");
            else
            {
                _unitOfWork.ScheduleRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa lịch thất bại");
            }
        }

        public async Task<ScheduleViewModel> GetScheduleById(Guid ScheduleId)
        {
            var result = await _unitOfWork.ScheduleRepository.GetByIdAsync(ScheduleId);
            var mapper = _mapper.Map<ScheduleViewModel>(result);
            return mapper;
        }

        public async Task<List<ScheduleViewModel>> GetSchedules()
        {
            var results = _unitOfWork.ScheduleRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<ScheduleViewModel>>(results);
            return mapper;
        }

        public async Task<bool> UpdateSchedule(UpdateScheduleViewModel updateScheduleViewModel)
        {
            var validator = new UpdateScheduleViewModelValidator();
            var validationResult = validator.Validate(updateScheduleViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var schedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(updateScheduleViewModel.Id);
            if (schedule == null)
            {
                throw new Exception("Không tìm thấy lịch");
            }

            var existingSchedule = await _unitOfWork.ScheduleRepository.GetScheduleByClass(updateScheduleViewModel.ClassId);
            if (!existingSchedule.IsNullOrEmpty())
            {
                if (existingSchedule.FirstOrDefault().Id != updateScheduleViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }

            /* schedule.Name = updateScheduleViewModel.Name;
             schedule.Status = updateScheduleViewModel.Status;*/

            _unitOfWork.ScheduleRepository.Update(_mapper.Map(updateScheduleViewModel, schedule));
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật lịch thất bại");
        }

        /* public async Task<AutoScheduleViewModel> CreateAutomaticalySchedule()
         {
             int countSchedule = 0;
             int countRoom = 0;
             var slots = _unitOfWork.SlotRepository.GetAllAsync().Result.Where(x => x.SlotType == SlotType.Course).ToList();
             for (var i = 1; i <= slots.Count; i++)
             {
                 var classes = await _unitOfWork.ClassRepository.GetClassBySlot(i); //lấy list lớp học theo từng slot
                 var fullTeachers = await _unitOfWork.UserRepository.GetTeacherByJobType(JobType.FullTime); //lấy list giáo viên fulltime
                 var partTeachers = await _unitOfWork.UserRepository.GetTeacherByJobType(JobType.PartTime); //lấy list giáo viên parttime
                 var rooms = await _unitOfWork.RoomRepository.GetRoomByStatus(StatusOfRoom.Empty);
                 foreach (var item in classes)
                 {
                     if (fullTeachers.Count != 0)
                     {
                         foreach (var teacher in fullTeachers)
                         {

                             var countSlotPerWeek = (await _unitOfWork.TeachingClassHistoryRepository
                                 .GetClassByTeacherId(teacher.Id))
                                 .Count * 2;//2 là số slot học trong 1 tuần của 1 lớp, có thể thay đổi
                             if (countSlotPerWeek < 24)//24 là số slot dạy trong 1 tuần, slotperweek trong configjobtype
                             {
                                 var teachingHistory = new TeachingClassHistory
                                 {
                                     ClassId = item.Id,
                                     UserAccountId = teacher.Id,
                                     StartDate = _currentTime.GetCurrentTime(),
                                     TeachingStatus = TeachingStatus.Pending
                                 };
                                 await _unitOfWork.TeachingClassHistoryRepository.AddAsync(teachingHistory);
                                 fullTeachers.Remove(teacher);
                                 break;
                             }

                         }
                     }
                     else if (partTeachers.Count != 0)
                     {
                         foreach (var teacher in partTeachers)
                         {

                             var countSlotPerWeek = (await _unitOfWork.TeachingClassHistoryRepository
                                 .GetClassByTeacherId(teacher.Id)).Count * 2;//2 là số slot học 1 lớp trong 1 tuần
                             if (countSlotPerWeek < 15)//15 là số slot dạy trong 1 tuần, slotperweek trong configjobtype
                             {
                                 var teachingHistory = new TeachingClassHistory
                                 {
                                     ClassId = item.Id,
                                     UserAccountId = teacher.Id,
                                     StartDate = _currentTime.GetCurrentTime(),
                                     TeachingStatus = TeachingStatus.Pending
                                 };
                                 await _unitOfWork.TeachingClassHistoryRepository.AddAsync(teachingHistory);
                                 partTeachers.Remove(teacher);
                                 break;
                             }
                         }
                     }
                     else
                     {
                         countSchedule++;
                     }

                     if (rooms.Count != 0)
                     {
                         foreach (var room in rooms)
                         {
                             foreach (var perSchedule in item.Schedules)
                             {
                                 var scheduleRoom = new ScheduleRoom
                                 {
                                     RoomId = room.Id,
                                     ScheduleId = perSchedule.Id
                                 };

                                 await _unitOfWork.ScheduleRoomRepository.AddAsync(scheduleRoom);
                             }
                             rooms.Remove(room);
                             break;
                         }
                     }
                     else
                     {
                         countRoom++;
                     }
                 }

             }

             await _unitOfWork.SaveChangeAsync();

             return new AutoScheduleViewModel
             {
                 CountSchedule = countSchedule,
                 CountRoom = countRoom
             };
         }*/

        public async Task<List<AutoScheduleViewModel>> CreateAutomaticalySchedule()
        {
            // bắt đầu đoạn code lấy data và check điều kiện xếp lịch
            var slots = _unitOfWork.SlotRepository.GetAllAsync().Result.Where(x => x.SlotType == SlotType.Course).ToList();

            var rooms = await _unitOfWork.RoomRepository.GetRoomByStatus(StatusOfRoom.Empty); //lấy list phòng trống
            var fullTeachers = await _unitOfWork.UserRepository.GetTeacherByJobType(JobType.FullTime); //lấy list giáo viên fulltime
            var partTeachers = await _unitOfWork.UserRepository.GetTeacherByJobType(JobType.PartTime); //lấy list giáo viên parttime
            var checkClasses = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.StatusOfClass == StatusOfClass.Pending
            && x.TeachingClassHistories.Count == 0 && x.Schedules.Count != 0).ToList();

            if (rooms.IsNullOrEmpty())
            {
                throw new Exception("Không có phòng trống để xếp lịch");
            }
            else if (checkClasses.IsNullOrEmpty())
            {
                throw new Exception("Danh sách lớp cần xếp lịch đã hết");
            }
            else if (fullTeachers.IsNullOrEmpty() && partTeachers.IsNullOrEmpty())
            {
                throw new Exception("Không đủ giáo viên để xếp lịch");
            }
            // kết thúc đoạn code lấy data và check điều kiện xếp lịch

            //add 2 list giáo viên vào Queue để xếp xoay vòng
            Queue<UserAccount> fullTimeQueue = new();
            Queue<UserAccount> partTimeQueue = new();
            foreach (var fullTeacher in fullTeachers)
            {
                var listFilterFull = await _unitOfWork.TeachingClassHistoryRepository.GetClassByTeacherId(fullTeacher.Id);
                if (listFilterFull.Count == 0) // chỉ lấy list những gv rảnh, tức nếu pending/teaching thì k add vô queue
                {
                    fullTimeQueue.Enqueue(fullTeacher);
                }
            }
            foreach (var partTeacher in partTeachers)
            {
                var listFilterPart = await _unitOfWork.TeachingClassHistoryRepository.GetClassByTeacherId(partTeacher.Id);
                if (listFilterPart.Count == 0) // chỉ lấy list những gv rảnh, tức nếu pending/teaching thì k add vô queue
                {
                    partTimeQueue.Enqueue(partTeacher);
                }
            }

            /*
            // bắt đầu đoạn code check đã tạo lịch hay chưa để tiếp tục xếp lịch hay văng lỗi
            var pendingHistory = await _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByStatus(TeachingStatus.Pending);
            var anotherStatusHistory = _unitOfWork.TeachingClassHistoryRepository.GetAllAsync().Result.Any(x => x.TeachingStatus == TeachingStatus.Teaching || x.TeachingStatus == TeachingStatus.Substitute);
            var pendingScheduleRoom = await _unitOfWork.ScheduleRoomRepository.GetScheduleRoomByStatus(ScheduleRoomStatus.Pending);
            var anotherStatusScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetAllAsync().Result.Any(x => x.Status == ScheduleRoomStatus.Using || x.Status == ScheduleRoomStatus.Temp);

            if (anotherStatusHistory || anotherStatusScheduleRoom) //nếu có bất kì lịch hoặc phòng nào được xếp trước đó mà đã hoạt động
            {
                throw new Exception("Xếp lịch không thành công, đã có lịch hoặc phòng được xếp trước đó và đã hoạt động");
            }
            else // ngược lại khi cả lịch và phòng chỉ mới được xếp mà chưa hoạt động (pending) thì xóa đi xếp lại 
            {
                _unitOfWork.TeachingClassHistoryRepository.RemoveRange(pendingHistory);
                _unitOfWork.ScheduleRoomRepository.RemoveRange(pendingScheduleRoom);
            }
            // kết thúc đoạn code check đã tạo lịch hay chưa
            */

            List<AutoScheduleViewModel> list = new();
            var tempFullTeachers = new Queue<UserAccount>();
            var tempPartTeachers = new Queue<UserAccount>();

            // băt đầu xếp lịch
            for (var i = 1; i <= slots.Count; i++) // cho vòng lặp chạy theo từng slot
            {
                rooms = await _unitOfWork.RoomRepository.GetRoomByStatus(StatusOfRoom.Empty); //reset list phòng trống
                var classes = await _unitOfWork.ClassRepository.GetClassBySlot(i); //lấy list lớp học pending theo từng slot

                //add lại queue chính từ queue tạm cho full/part
                while (tempFullTeachers.Count > 0)
                {
                    fullTimeQueue.Enqueue(tempFullTeachers.Dequeue());

                }

                while (tempPartTeachers.Count > 0)
                {
                    partTimeQueue.Enqueue(tempPartTeachers.Dequeue());

                }

                if (i != (slots.Count)) // nếu không phải là slot cuối
                {
                    int countSchedule = 0;
                    List<ClassViewModel> listClassCount = new();
                    List<ClassViewModel> listRoomCount = new();
                    int countRoom = 0;

                    foreach (var item in classes)
                    {
                        if (fullTimeQueue.Count != 0)
                        {
                            var teacher = fullTimeQueue.Dequeue(); //lấy teacher ra khỏi queue

                            var teachingHistory = new TeachingClassHistory
                            {
                                ClassId = item.Id,
                                UserAccountId = teacher.Id,
                                StartDate = (DateTime)item.Schedules.FirstOrDefault().StartDate,
                                TeachingStatus = TeachingStatus.Pending
                            };
                            await _unitOfWork.TeachingClassHistoryRepository.AddAsync(teachingHistory);

                            tempFullTeachers.Enqueue(teacher); //add lại teacher đã xếp vào list tạm
                        }
                        else if (partTimeQueue.Count != 0)
                        {
                            var teacher = partTimeQueue.Dequeue();

                            var teachingHistory = new TeachingClassHistory
                            {
                                ClassId = item.Id,
                                UserAccountId = teacher.Id,
                                StartDate = (DateTime)item.Schedules.FirstOrDefault().StartDate,
                                TeachingStatus = TeachingStatus.Pending
                            };
                            await _unitOfWork.TeachingClassHistoryRepository.AddAsync(teachingHistory);

                            tempPartTeachers.Enqueue(teacher);
                        }
                        else
                        {
                            listClassCount.Add(_mapper.Map<ClassViewModel>(item));
                            countSchedule++;
                        }

                        if (rooms.Count != 0)
                        {
                            foreach (var room in rooms) //duyệt danh sách phòng
                            {
                                foreach (var perSchedule in item.Schedules) //mỗi lớp học 2 slot nên có 2 lịch xếp cho học 1 phòng
                                {
                                    var scheduleRoom = new ScheduleRoom
                                    {
                                        RoomId = room.Id,
                                        ScheduleId = perSchedule.Id,
                                        StartDate = (DateTime)item.Schedules.FirstOrDefault().StartDate,
                                        Status = ScheduleRoomStatus.Pending
                                    };

                                    await _unitOfWork.ScheduleRoomRepository.AddAsync(scheduleRoom);
                                }
                                rooms.Remove(room);
                                break;
                            }
                        }
                        else
                        {
                            listRoomCount.Add(_mapper.Map<ClassViewModel>(item));
                            countRoom++;
                        }
                    }

                    if (countRoom != 0 || countSchedule != 0)
                    {
                        list.Add(new AutoScheduleViewModel()
                        {
                            Slot = i,
                            CountSchedule = countSchedule,
                            CountRoom = countRoom,
                            ListClassCount = listClassCount,
                            ListRoomCount = listRoomCount
                        });
                    }
                }
                else //khi là slot cuối thì chỉ xếp lịch cho gv partTime thôi
                {
                    int countSchedule = 0;
                    List<ClassViewModel> listClassCount = new();
                    List<ClassViewModel> listRoomCount = new();
                    int countRoom = 0;

                    foreach (var item in classes)
                    {
                        if (partTimeQueue.Count != 0)
                        {
                            var teacher = partTimeQueue.Dequeue();

                            var teachingHistory = new TeachingClassHistory
                            {
                                ClassId = item.Id,
                                UserAccountId = teacher.Id,
                                StartDate = (DateTime)item.Schedules.FirstOrDefault().StartDate,
                                TeachingStatus = TeachingStatus.Pending
                            };
                            await _unitOfWork.TeachingClassHistoryRepository.AddAsync(teachingHistory);

                            tempPartTeachers.Enqueue(teacher);


                        }
                        else if (fullTimeQueue.Count != 0)
                        {
                            var teacher = fullTimeQueue.Dequeue();

                            var teachingHistory = new TeachingClassHistory
                            {
                                ClassId = item.Id,
                                UserAccountId = teacher.Id,
                                StartDate = (DateTime)item.Schedules.FirstOrDefault().StartDate,
                                TeachingStatus = TeachingStatus.Pending
                            };
                            await _unitOfWork.TeachingClassHistoryRepository.AddAsync(teachingHistory);
                            partTeachers.Remove(teacher);

                            tempFullTeachers.Enqueue(teacher);
                        }
                        else
                        {
                            listClassCount.Add(_mapper.Map<ClassViewModel>(item));
                            countSchedule++;
                        }

                        if (rooms.Count != 0)
                        {
                            foreach (var room in rooms)
                            {
                                foreach (var perSchedule in item.Schedules)
                                {
                                    var scheduleRoom = new ScheduleRoom
                                    {
                                        RoomId = room.Id,
                                        ScheduleId = perSchedule.Id,
                                        StartDate = (DateTime)item.Schedules.FirstOrDefault().StartDate,
                                        Status = ScheduleRoomStatus.Pending
                                    };

                                    await _unitOfWork.ScheduleRoomRepository.AddAsync(scheduleRoom);
                                }
                                rooms.Remove(room);
                                break;
                            }
                        }
                        else
                        {
                            listRoomCount.Add(_mapper.Map<ClassViewModel>(item));
                            countRoom++;
                        }
                    }

                    if (countRoom != 0 || countSchedule != 0)
                    {
                        list.Add(new AutoScheduleViewModel()
                        {
                            Slot = i,
                            CountSchedule = countSchedule,
                            CountRoom = countRoom,
                            ListClassCount = listClassCount,
                            ListRoomCount = listRoomCount
                        });
                    }
                }
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? list : throw new Exception("Tạo lịch thất bại, hãy đảm bảo đã có lớp, lịch, phòng và giáo viên");
        }

        public async Task<GetAutoScheduleViewModel> GetAutomaticalySchedule(Guid id)
        {
            // hiện tại mỗi lịch riêng biệt và có lịch sử scheduleRoom riêng, trong trường hợp có nhiều room (chuyển phòng)
            // thì vẫn là 1 lịch có nhiều room, lúc đó hãy cứ lấy ra hết tất cả scheduleRoom theo scheduleId
            // sẽ có cái expired thật sự, đã expired nhưng vẫn chưa tới ngày end, using hoặc temp,
            // get cả lịch expired với endDate > now, tức chưa tới ngày end, concat với lịch using và temp là ra được 
            // list cần để hiện thị lịch mà lớp đó đang học ( loại bỏ được lịch đã expired với endDate đã qua)
            // trả thêm startDate và endDate trong viewModel
            // mỗi lớp 1 course 1 ngày chỉ học 1 slot, tức nếu có người dạy thay thì lịch sẽ k hiển thị ngày hôm đó

            var getAutoSchedule = new GetAutoScheduleViewModel();
            var classesModel = new List<ClassForScheduleViewModel>();
            //var slotModel = new SlotForScheduleViewModel();

            // lấy lịch sử những lớp gv đang dạy hoặc được xếp lịch (pending, teaching)
            var histories = await _unitOfWork.TeachingClassHistoryRepository.GetClassByTeacherId(id);

            foreach (var history in histories)
            {
                var schedulesModel = new List<ScheduleForAutoViewModel>();
                var mapper = _mapper.Map<ClassForScheduleViewModel>(history.Class);
                mapper.TeachingStartDate = history.StartDate;
                mapper.TeachingEndDate = history.EndDate;

                if (history.EndDate.Date.ToString("yyyy-dd-MM") != "0001-01-01")
                {
                    int daysDifference = (int)(history.EndDate - history.StartDate).TotalDays; // Số ngày giữa hai ngày
                    int totalWeeks;

                    if (daysDifference % 7 == 0) // Nếu chia hết cho 7
                    {
                        totalWeeks = (daysDifference / 7) * 2 + 1;
                    }
                    else
                    {
                        totalWeeks = (int)Math.Ceiling((double)daysDifference / 7) * 2;
                    }

                    mapper.TotalDuration = totalWeeks;
                }
                else
                {
                    // lấy list những teachingHistory của lớp đó ra để đếm totalDuration
                    var beforeHistories = await _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByClassId(history.ClassId);
                    int duration = 0;
                    foreach (var item in beforeHistories)
                    {
                        if (item.EndDate.Date.ToString("yyyy-dd-MM") != "0001-01-01")
                        {
                            int daysDifference = (int)(item.EndDate - item.StartDate).TotalDays; // Số ngày giữa hai ngày
                            int totalWeeks;

                            if (daysDifference % 7 == 0) // Nếu chia hết cho 7
                            {
                                totalWeeks = (daysDifference / 7) * 2 + 1;
                                duration += totalWeeks;
                            }
                            else
                            {
                                totalWeeks = (int)Math.Ceiling((double)daysDifference / 7) * 2;
                                duration += totalWeeks;
                            }
                        }
                    }

                    mapper.TotalDuration = history.Class.Course.DurationTotal - duration;
                }

                var schedules = await _unitOfWork.ScheduleRepository.GetScheduleByClass(history.ClassId);
                foreach (var schedule in schedules)
                {
                    var roomsModel = new List<RoomForScheduleViewModel>();
                    var sm = _mapper.Map<ScheduleForAutoViewModel>(schedule);
                    var slot = _unitOfWork.SlotRepository.GetAllAsync().Result.FirstOrDefault(x => x.Id == schedule.SlotId && x.IsDeleted == false);
                    var scheduleRooms = _unitOfWork.ScheduleRoomRepository.GetAllAsync().Result
                        .Where(x => x.ScheduleId == schedule.Id && x.IsDeleted == false && x.Status != ScheduleRoomStatus.Expired).ToList();

                    foreach (var scheduleRoom in scheduleRooms)
                    {
                        roomsModel.Add(_mapper.Map<RoomForScheduleViewModel>(await _unitOfWork.RoomRepository.GetByIdAsync((Guid)scheduleRoom.RoomId)));
                    }

                    sm.Slot = _mapper.Map<SlotForScheduleViewModel>(slot);
                    sm.Rooms = roomsModel;
                    schedulesModel.Add(sm);

                }
                mapper.Schedules = schedulesModel;
                classesModel.Add(mapper);

            }
            getAutoSchedule.TeacherId = id;
            getAutoSchedule.Classes = classesModel;

            return getAutoSchedule;
        }


        public async Task<ScheduleRoomAndTeachingClassHistoryViewModel> GetScheduleRoomAndTeachingClassHistory()
        {

            var histories1 = await _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByStatus(TeachingStatus.Pending);
            var histories2 = await _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByStatus(TeachingStatus.Substitute);
            var histories3 = await _unitOfWork.TeachingClassHistoryRepository.GetTeachingHistoryByStatus(TeachingStatus.Teaching);
            var scheduleRooms1 = await _unitOfWork.ScheduleRoomRepository.GetScheduleRoomByStatus(ScheduleRoomStatus.Pending);
            var scheduleRooms2 = await _unitOfWork.ScheduleRoomRepository.GetScheduleRoomByStatus(ScheduleRoomStatus.Using);
            var scheduleRooms3 = await _unitOfWork.ScheduleRoomRepository.GetScheduleRoomByStatus(ScheduleRoomStatus.Temp);

            ScheduleRoomAndTeachingClassHistoryViewModel model = new ScheduleRoomAndTeachingClassHistoryViewModel();
            model.TeachingClassHistories = histories1.Concat(histories2).Concat(histories3).ToList();
            model.ScheduleRooms = scheduleRooms1.Concat(scheduleRooms2).Concat(scheduleRooms3).ToList();
            return model;
        }

        // dùng cho api đổi phòng cho lịch học, vừa tạm thời vừa đổi luôn
        public async Task<bool> ChangeRoomForSchedule(ChangeRoomForScheduleViewModel changeRoomForScheduleViewModel)
        {

            // thêm api lấy ra những phòng còn trống trong cùng slot đó để hiện thị lên trên view thay vì cho chọn r xuống
            // dưới BE bắt
            // trạng thái của scheduleRoom: pending, using, temp, expired 
            // chỉ cho chuyển với trạng thái pending, using, temp
            // chuyển qua phòng trống trong cùng slot
            // làm sao để trả ra được lịch đó học phòng đó ngày nào trong trường hợp học tạm
            // => có startDate, startDate sẽ:
            // đổi tạm thì s => đổi tạm có 2 phương hướng là đóng record cũ ghi 2 record mới vừa đổi tạm vừa ghi lại phòng cũ
            //                  hoặc vẫn giữ record cũ và chỉ thêm 1 record đổi tạm mới
            // đổi luôn thì s => đổi luôn thì update status scheduleroom cũ, update endDate, ghi thêm record scheduleroom mới
            // với starDate và status pending

            // lưu ý: cả lịch và phòng đổi là sẽ thêm record mới kể cả tạm 1 lịch
            // -------------------------------------


            switch (changeRoomForScheduleViewModel.Status)
            {
                /*case ScheduleRoomStatus.Pending:
                    break;*/
                case ScheduleRoomStatus.Using: // sử dụng trạng thái using để nhân biết rằng lịch đổi phòng luôn
                    {
                        // lấy ra list những lịch có ngày kết thúc sau startDate của lịch mới, tức đổi từ startDate mới đổi đi
                        var listOldScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(changeRoomForScheduleViewModel.ScheduleId)
                            .Result.Where(x => x.EndDate.Value.Date > changeRoomForScheduleViewModel.StartDate.Date
                            && x.IsDeleted == false && x.Status != ScheduleRoomStatus.Expired).ToList();
                        var endDate = new DateTime();

                        // nếu startDate trong lịch mới là những ngày này thì endDate của lịch cũ sẽ trừ đi số ngày tương ứng
                        if (changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("thursday") // 5 8
                            || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("tuesday") // 3 6
                            || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("wednesday")) // 4 7
                        {
                            // là những ngày đầu tiên trong tuần thì trừ cho 4 để ra ngày tuần trước
                            endDate = changeRoomForScheduleViewModel.StartDate.AddDays(-4);
                        }
                        else
                        {
                            endDate = changeRoomForScheduleViewModel.StartDate.AddDays(-3);
                        }

                        // cập nhật endDate cho những lịch trước đó
                        foreach (var scheduleRoom in listOldScheduleRoom)
                        {
                            scheduleRoom.EndDate = endDate;
                            _unitOfWork.ScheduleRoomRepository.Update(scheduleRoom);
                        }

                        // tạo record mới cho phòng mới
                        var newScheduleRoom = new ScheduleRoom()
                        {
                            ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                            RoomId = changeRoomForScheduleViewModel.NewRoomId,
                            StartDate = _currentTime.GetCurrentTime(),
                            Status = ScheduleRoomStatus.Using
                        };

                        await _unitOfWork.ScheduleRoomRepository.AddAsync(newScheduleRoom);
                    }

                    break;
                case ScheduleRoomStatus.Temp: // dùng status temp để nhận biết chuyển phòng tạm thời
                    {
                        var listScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomByScheduleAndRoom(changeRoomForScheduleViewModel.ScheduleId, changeRoomForScheduleViewModel.RoomId)
                            .Result.Where(x => x.Status != ScheduleRoomStatus.Expired).OrderByDescending(x => x.CreationDate);

                        // nếu trùng ngày bắt đầu
                        if (listScheduleRoom.Any(x => x.StartDate.Value.Date == changeRoomForScheduleViewModel.StartDate.Date))
                        {
                            var currentScheduleRoom = listScheduleRoom.FirstOrDefault(x => x.StartDate.Value.Date == changeRoomForScheduleViewModel.StartDate.Date);

                            // nếu ngày kết thúc của lịch cũ trước ngày kết thúc của lịch mới thì sửa lại startDate của lịch cũ và 
                            // thêm lịch mới
                            if (currentScheduleRoom.EndDate.Value.Date < changeRoomForScheduleViewModel.EndDate.Value.Date)
                            {
                                // nếu là những ngày này thì ngày bắt đầu của lịch cũ phải là ngày kết thúc mới cộng thêm 3 else +4
                                if (currentScheduleRoom.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("tuesday")
                                    || currentScheduleRoom.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("wednesday")
                                    || currentScheduleRoom.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("thursday"))
                                {

                                    currentScheduleRoom.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(3);
                                }
                                else
                                {
                                    currentScheduleRoom.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(4);

                                }

                                var scheduleRoom = new ScheduleRoom()
                                {
                                    ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                    RoomId = changeRoomForScheduleViewModel.NewRoomId,
                                    StartDate = changeRoomForScheduleViewModel.StartDate,
                                    EndDate = changeRoomForScheduleViewModel.EndDate,
                                    Status = ScheduleRoomStatus.Temp
                                };

                                await _unitOfWork.ScheduleRoomRepository.AddAsync(scheduleRoom);
                                _unitOfWork.ScheduleRoomRepository.Update(currentScheduleRoom);
                            }
                            else // ngược lại dù trùng hay hơn ngày kết thúc thì đều update lại lịch cũ được
                            {
                                currentScheduleRoom.StartDate = changeRoomForScheduleViewModel.StartDate;
                                currentScheduleRoom.EndDate = changeRoomForScheduleViewModel.EndDate;

                                _unitOfWork.ScheduleRoomRepository.Update(currentScheduleRoom);
                            }
                        }
                        // nếu không trùng ngày bắt đầu
                        else
                        {
                            // lịch mới nằm giữa 1 lịch cũ
                            if (listScheduleRoom.Any(x => x.StartDate.Value.Date < changeRoomForScheduleViewModel.StartDate
                                    && x.EndDate.Value.Date > changeRoomForScheduleViewModel.EndDate.Value.Date))
                            {
                                var currentScheduleRoom = listScheduleRoom.FirstOrDefault(x => x.StartDate.Value.Date < changeRoomForScheduleViewModel.StartDate
                                    && x.EndDate.Value.Date > changeRoomForScheduleViewModel.EndDate.Value.Date);

                                var endDateOld = currentScheduleRoom.EndDate;

                                if (changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("tuesday")
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("wednesday")
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("thursday"))
                                {
                                    currentScheduleRoom.EndDate = changeRoomForScheduleViewModel.StartDate.AddDays(-4);
                                }
                                else
                                {
                                    currentScheduleRoom.EndDate = changeRoomForScheduleViewModel.StartDate.AddDays(-3);
                                }

                                //  thêm lịch tạm mới
                                var newTempScheduleRoom = new ScheduleRoom()
                                {
                                    ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                    RoomId = changeRoomForScheduleViewModel.NewRoomId,
                                    StartDate = changeRoomForScheduleViewModel.StartDate,
                                    EndDate = changeRoomForScheduleViewModel.EndDate,
                                    Status = ScheduleRoomStatus.Temp
                                };

                                // thêm lại lịch tạm cũ
                                var newScheduleRoom = new ScheduleRoom()
                                {
                                    ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                    RoomId = changeRoomForScheduleViewModel.RoomId,
                                    EndDate = endDateOld,
                                    Status = ScheduleRoomStatus.Temp
                                };

                                if (changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("tuesday")
                                        || changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("wednesday")
                                        || changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("thursday"))
                                {

                                    newScheduleRoom.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(3);
                                }
                                else
                                {
                                    newScheduleRoom.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(4);

                                }

                                await _unitOfWork.ScheduleRoomRepository.AddAsync(newTempScheduleRoom);
                                await _unitOfWork.ScheduleRoomRepository.AddAsync(newScheduleRoom);
                                _unitOfWork.ScheduleRoomRepository.Update(currentScheduleRoom);
                            }
                            else // kiểm tra vị trí lịch so với các lịch cũ
                            {
                                // không cần lấy endDate bởi vì nếu cả start và end đều trong khoảng thì dính trường hợp trên
                                var scheduleRoomOld1 = listScheduleRoom.FirstOrDefault(x => x.StartDate.Value.Date < changeRoomForScheduleViewModel.StartDate.Date
                                && x.EndDate.Value.Date > changeRoomForScheduleViewModel.StartDate.Date);

                                var scheduleRoomOld2 = listScheduleRoom.FirstOrDefault(x => x.StartDate.Value.Date < changeRoomForScheduleViewModel.EndDate.Value.Date
                                && x.EndDate.Value.Date > changeRoomForScheduleViewModel.EndDate.Value.Date);

                                //lịch mới nằm ở giữa 2 lịch(--__--)
                                // cập nhât lại ngày kết thúc của cũ trước, ngày bắt đầu của cũ sau, thêm lịch mới
                                if (scheduleRoomOld1 != null && scheduleRoomOld2 != null)
                                {
                                    if (changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("tuesday")
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("wednesday")
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("thursday"))
                                    {

                                        scheduleRoomOld1.EndDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(-4);
                                    }
                                    else
                                    {
                                        scheduleRoomOld1.EndDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(-3);

                                    }

                                    if (changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("tuesday")
                                        || changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("wednesday")
                                        || changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("thursday"))
                                    {

                                        scheduleRoomOld2.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(3);
                                    }
                                    else
                                    {
                                        scheduleRoomOld2.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(4);

                                    }

                                    var newTempScheduleRoom = new ScheduleRoom()
                                    {
                                        ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                        RoomId = changeRoomForScheduleViewModel.NewRoomId,
                                        StartDate = changeRoomForScheduleViewModel.StartDate,
                                        EndDate = changeRoomForScheduleViewModel.EndDate,
                                        Status = ScheduleRoomStatus.Temp
                                    };

                                    await _unitOfWork.ScheduleRoomRepository.AddAsync(newTempScheduleRoom);
                                    _unitOfWork.ScheduleRoomRepository.Update(scheduleRoomOld1);
                                    _unitOfWork.ScheduleRoomRepository.Update(scheduleRoomOld2);

                                }
                                else
                                // lịch mới nằm lệch 1 trong 2 lịch (--__)(__--)
                                if (scheduleRoomOld1 != null && scheduleRoomOld2 == null)
                                {
                                    if (changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("tuesday")
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("wednesday")
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("thursday"))
                                    {

                                        scheduleRoomOld1.EndDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(-4);
                                    }
                                    else
                                    {
                                        scheduleRoomOld1.EndDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(-3);

                                    }

                                    var newTempScheduleRoom = new ScheduleRoom()
                                    {
                                        ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                        RoomId = changeRoomForScheduleViewModel.NewRoomId,
                                        StartDate = changeRoomForScheduleViewModel.StartDate,
                                        EndDate = changeRoomForScheduleViewModel.EndDate,
                                        Status = ScheduleRoomStatus.Temp
                                    };

                                    await _unitOfWork.ScheduleRoomRepository.AddAsync(newTempScheduleRoom);
                                    _unitOfWork.ScheduleRoomRepository.Update(scheduleRoomOld1);
                                }
                                else
                                if (scheduleRoomOld1 == null && scheduleRoomOld2 != null)
                                {
                                    if (changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("tuesday")
                                        || changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("wednesday")
                                        || changeRoomForScheduleViewModel.EndDate.Value.DayOfWeek.ToString().ToLower().Equals("thursday"))
                                    {

                                        scheduleRoomOld2.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(3);
                                    }
                                    else
                                    {
                                        scheduleRoomOld2.StartDate = changeRoomForScheduleViewModel.EndDate.Value.AddDays(4);

                                    }

                                    var newTempScheduleRoom = new ScheduleRoom()
                                    {
                                        ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                        RoomId = changeRoomForScheduleViewModel.NewRoomId,
                                        StartDate = changeRoomForScheduleViewModel.StartDate,
                                        EndDate = changeRoomForScheduleViewModel.EndDate,
                                        Status = ScheduleRoomStatus.Temp
                                    };

                                    await _unitOfWork.ScheduleRoomRepository.AddAsync(newTempScheduleRoom);
                                    _unitOfWork.ScheduleRoomRepository.Update(scheduleRoomOld2);
                                }
                                else
                                // lịch mới độc lập với các lịch còn lại, add như bình thường
                                if (scheduleRoomOld1 == null && scheduleRoomOld2 == null)
                                {
                                    // lịch cũ đã được sort theo ngày tạo để lấy mới nhất
                                    var currentScheduleRoom = listScheduleRoom.FirstOrDefault();

                                    // nếu startDate trong lịch mới là những ngày này thì endDate của lịch cũ sẽ trừ đi số ngày tương ứng
                                    if (changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("thursday") // 5 8
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("tuesday") // 3 6
                                        || changeRoomForScheduleViewModel.StartDate.DayOfWeek.ToString().ToLower().Equals("wednesday")) // 4 7
                                    {
                                        // là những ngày đầu tiên trong tuần thì trừ cho 4 để ra ngày tuần trước
                                        currentScheduleRoom.EndDate = changeRoomForScheduleViewModel.StartDate.AddDays(-4);

                                        // add record mới cho phòng tạm
                                        var tempScheduleRoom = new ScheduleRoom()
                                        {
                                            ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                            RoomId = changeRoomForScheduleViewModel.NewRoomId,
                                            StartDate = changeRoomForScheduleViewModel.StartDate,
                                            EndDate = changeRoomForScheduleViewModel.StartDate,
                                            Status = ScheduleRoomStatus.Temp
                                        };

                                        // add lại record phòng cho lịch cũ
                                        var newScheduleRoomAgain = new ScheduleRoom()
                                        {
                                            ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                            RoomId = currentScheduleRoom.RoomId,
                                            StartDate = changeRoomForScheduleViewModel.StartDate.AddDays(3),
                                            Status = ScheduleRoomStatus.Using
                                        };

                                        await _unitOfWork.ScheduleRoomRepository.AddAsync(tempScheduleRoom);
                                        await _unitOfWork.ScheduleRoomRepository.AddAsync(newScheduleRoomAgain);
                                        _unitOfWork.ScheduleRoomRepository.Update(currentScheduleRoom);

                                    }
                                    else
                                    {
                                        currentScheduleRoom.EndDate = changeRoomForScheduleViewModel.StartDate.AddDays(-3);

                                        // add record mới cho phòng tạm
                                        var tempScheduleRoom = new ScheduleRoom()
                                        {
                                            ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                            RoomId = changeRoomForScheduleViewModel.NewRoomId,
                                            StartDate = changeRoomForScheduleViewModel.StartDate,
                                            EndDate = changeRoomForScheduleViewModel.StartDate,
                                            Status = ScheduleRoomStatus.Temp
                                        };

                                        // add lại record phòng cho lịch cũ
                                        var newScheduleRoomAgain = new ScheduleRoom()
                                        {
                                            ScheduleId = changeRoomForScheduleViewModel.ScheduleId,
                                            RoomId = currentScheduleRoom.RoomId,
                                            StartDate = changeRoomForScheduleViewModel.StartDate.AddDays(4),
                                            Status = ScheduleRoomStatus.Using
                                        };

                                        await _unitOfWork.ScheduleRoomRepository.AddAsync(tempScheduleRoom);
                                        await _unitOfWork.ScheduleRoomRepository.AddAsync(newScheduleRoomAgain);
                                        _unitOfWork.ScheduleRoomRepository.Update(currentScheduleRoom);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Trường hợp chưa hỗ trợ");
                                }
                            }
                        }
                    }

                    break;
                /*case ScheduleRoomStatus.Expired:
                    break;*/
                default:
                    throw new Exception("Trạng thái không phù hợp");
            }

            // xóa list những lịch lỗi sau khi đổi nhiều lần có ngày start sau ngày end
            var listScheduleRoomError = _unitOfWork.ScheduleRoomRepository.GetAllAsync().Result
                .Where(x => x.StartDate.Value.Date > x.EndDate.Value.Date).ToList();
            _unitOfWork.ScheduleRoomRepository.RemoveRange(listScheduleRoomError);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Chuyển phòng thất bại");
        }

        public async Task<bool> GetEmptyRoomBySlot(Guid scheduleId, Guid slotId, DateTime startDate, DateTime endDate, ScheduleRoomStatus status)
        {
            // lấy list phòng học trống theo slot ra mà không biết có cần phải lấy theo ngày không,
            // 1 slot có nhiều lớp học, nếu học chính thì đỡ nhưng có những lớp đổi phòng nên có khi lại vướng vào ngày hôm đó
            // lấy theo status đi, muốn chuyển tạm thì lấy phòng kiểu tạm mà chuyển luôn thì lại khác nên lấy kiểu luôn

            //--------------------------------------------

            /*var schedules = await _unitOfWork.ScheduleRepository.GetListScheduleBySlot(slotId);

            var listEmptyRoom = new List<Room>();
            var listBusyRoom = new List<Room>();

            foreach (var schedule in schedules)
            {
                // giờ theo logic lấy ra phòng khác với scheduleId thử trước đã
                if (schedule.Id != scheduleId)
                {

                    // list này chứa những lịch phòng không hết hạn (temp, using, pending) và
                    var listScheduleRoom = _unitOfWork.ScheduleRoomRepository.GetScheduleRoomBySchedule(schedule.Id)
                        .Result.Where(x => x.Status != ScheduleRoomStatus.Expired).ToList();

                    foreach (var item in listScheduleRoom)
                    {
                        if (item.Status == ScheduleRoomStatus.Temp && item.StartDate.Value.Date > startDate
                            && item.EndDate.Value.Date <)
                    }
                }
            }*/
            return true;
        }
    }
}
