using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Schedules;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
using KidProEdu.Application.ViewModels.SlotViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using MediatR;
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
        public async Task<bool> CreateSchedule(CreateScheduleViewModel createScheduleViewModel)
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
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo lịch thất bại");
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
            int countSchedule = 0;
            int countRoom = 0;

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

            var slots = _unitOfWork.SlotRepository.GetAllAsync().Result.Where(x => x.SlotType == SlotType.Course).ToList();

            var rooms = await _unitOfWork.RoomRepository.GetRoomByStatus(StatusOfRoom.Empty); //lấy list phòng trống
            var fullTeachers = await _unitOfWork.UserRepository.GetTeacherByJobType(JobType.FullTime); //lấy list giáo viên fulltime
            var partTeachers = await _unitOfWork.UserRepository.GetTeacherByJobType(JobType.PartTime); //lấy list giáo viên parttime

            //add 2 list giáo viên vào Queue để xếp xoay vòng
            Queue<UserAccount> fullTimeQueue = new();
            Queue<UserAccount> partTimeQueue = new();
            foreach (var fullTeacher in fullTeachers)
            {
                fullTimeQueue.Enqueue(fullTeacher);
            }
            foreach (var partTeacher in partTeachers)
            {
                partTimeQueue.Enqueue(partTeacher);
            }

            List<AutoScheduleViewModel> list = new();

            for (var i = 1; i <= slots.Count; i++)
            {
                var tempFullTeachers = new Queue<UserAccount>();
                var tempPartTeachers = new Queue<UserAccount>();
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
                            countRoom++;
                        }
                    }
                }
                else //khi là slot cuối thì chỉ xếp lịch cho gv partTime thôi
                {
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
                            countRoom++;
                        }
                    }
                }

                if (countRoom != 0 || countSchedule != 0)
                {
                    list.Add(new AutoScheduleViewModel()
                    {
                        Slot = i,
                        CountSchedule = countSchedule,
                        CountRoom = countRoom
                    });
                }
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? list : throw new Exception("Tạo lịch thất bại, hãy đảm bảo đã có lớp, lịch, phòng và giáo viên");
        }

        public async Task<GetAutoScheduleViewModel> GetAutomaticalySchedule()
        {
            var getAutoSchedule = new GetAutoScheduleViewModel();
            var classesModel = new List<ClassForScheduleViewModel>();
            var schedulesModel = new List<ScheduleForAutoViewModel>();
            //var slotModel = new SlotForScheduleViewModel();
            var histories = await _unitOfWork.TeachingClassHistoryRepository.GetClassByTeacherId(new Guid("B01AF4AE-0D7D-4A49-940D-08DC4A7E376A"));

            foreach (var history in histories)
            {
                var mapper = _mapper.Map<ClassForScheduleViewModel>(history.Class);
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
            if (histories == null) { }


            getAutoSchedule.TeacherId = new Guid("B01AF4AE-0D7D-4A49-940D-08DC4A7E376A");
            getAutoSchedule.Classes = classesModel;


            return getAutoSchedule;
        }


        public async Task<bool> ChangeRoomForSchedule(ChangeRoomForScheduleViewModel changeRoomForScheduleViewModel)
        {




            return true;
        }
    }
}
