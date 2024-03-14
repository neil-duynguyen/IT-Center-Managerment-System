using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Schedules;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
using KidProEdu.Domain.Entities;
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
            await _unitOfWork.ScheduleRepository.AddAsync(mapper);
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
    }
}
