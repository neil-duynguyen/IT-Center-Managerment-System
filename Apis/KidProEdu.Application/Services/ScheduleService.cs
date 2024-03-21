using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Schedules;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
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

            var schedule = await _unitOfWork.ScheduleRepository.GetScheduleByClass(createScheduleViewModel.ClassId);
            if (!schedule.IsNullOrEmpty())
            {
                throw new Exception("Lịch đã tồn tại");
            }

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

            _unitOfWork.ScheduleRepository.Update(schedule);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật lịch thất bại");
        }

        public async Task<AutoScheduleViewModel> AutomaticalySchedule()
        {
            int countSchedule = 0;
            int countRoom = 0;
            var slots = await _unitOfWork.SlotRepository.GetAllAsync();
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
                            var teachingHistory = new TeachingClassHistory
                            {
                                ClassId = item.Id,
                                UserAccountId = teacher.Id,
                                StartDate = _currentTime.GetCurrentTime()
                            };

                            var countSlotPerWeek = (await _unitOfWork.TeachingClassHistoryRepository
                                .GetClassByTeacherId(teacher.Id))
                                .Count * 2;//2 là số slot học trong 1 tuần của 1 lớp, có thể thay đổi
                            if (countSlotPerWeek < 24)//24 là số slot dạy trong 1 tuần, slotperweek trong configjobtype
                            {
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
                            var teachingHistory = new TeachingClassHistory
                            {
                                ClassId = item.Id,
                                UserAccountId = teacher.Id,
                                StartDate = _currentTime.GetCurrentTime()
                            };

                            var countSlotPerWeek = (await _unitOfWork.TeachingClassHistoryRepository
                                .GetClassByTeacherId(teacher.Id)).Count * 2;//2 là số slot học 1 lớp trong 1 tuần
                            if (countSlotPerWeek < 15)//15 là số slot dạy trong 1 tuần, slotperweek trong configjobtype
                            {
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
        }
    }
}
