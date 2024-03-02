using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Documents;
using KidProEdu.Application.Validations.Lessons;
using KidProEdu.Application.Validations.LogEquipments;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.LogEquipmentViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KidProEdu.Application.Services
{
    public class LogEquipmentService : ILogEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public LogEquipmentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateLogEquipment(CreateLogEquipmentViewModel createLogEquipmentViewModel)
        {
            var validator = new CreateLogEquipmentViewModelValidator();
            var validationResult = validator.Validate(createLogEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var logEquipments = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByName(createLogEquipmentViewModel.Name);
            if (!logEquipments.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var logEquipments1 = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByEquipmentId(createLogEquipmentViewModel.EquipmentId);
            if (!logEquipments1.IsNullOrEmpty())
            {
                throw new Exception("Thiết bị đã tồn tại");
            }

            var logEquipments2 = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByCode(createLogEquipmentViewModel.Code);
            if (!logEquipments2.IsNullOrEmpty())
            {
                throw new Exception("Code đã tồn tại");
            }

            var mapper = _mapper.Map<LogEquipment>(createLogEquipmentViewModel);
            await _unitOfWork.LogEquipmentRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo tài liệu thất bại");
        }

        public async Task<bool> DeleteLogEquipment(Guid id)
        {
            var result = await _unitOfWork.LogEquipmentRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy thiết bị");
            else
            {
                _unitOfWork.LogEquipmentRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa thiết bị thất bại");
            }
        }

        public async Task<LogEquipmentViewModel> GetLogEquipmentById(Guid id)
        {
            var results = await _unitOfWork.LogEquipmentRepository.GetByIdAsync(id);

            var mapper = _mapper.Map<LogEquipmentViewModel>(results);

            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipments()
        {
            var results = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(results);

            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByCode(string code)
        {
            var results = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByCode(code);

            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(results);

            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByEquipmentId(Guid equipmentId)
        {
            var results = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByEquipmentId(equipmentId);

            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(results);

            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByName(string name)
        {
            var results = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByName(name);

            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(results);

            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByRoomId(Guid roomId)
        {
            var results = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByRoomId(roomId);

            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(results);

            return mapper;
        }

        public async Task<List<LogEquipmentViewModel>> GetLogEquipmentsByUserId(Guid userId)
        {
            var results = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByUserId(userId);

            var mapper = _mapper.Map<List<LogEquipmentViewModel>>(results);

            return mapper;
        }

        public async Task<bool> UpdateLogEquipment(UpdateLogEquipmentViewModel updateLogEquipmentViewModel)
        {
            var validator = new UpdateLogEquipmentViewModelValidator();
            var validationResult = validator.Validate(updateLogEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var logEquipment = await _unitOfWork.LogEquipmentRepository.GetByIdAsync(updateLogEquipmentViewModel.Id);
            if (logEquipment == null)
            {
                throw new Exception("Không tìm thấy bài học");
            }

            var existingLogEquipment = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByName(updateLogEquipmentViewModel.Name);
            if (!existingLogEquipment.IsNullOrEmpty())
            {
                if (existingLogEquipment.FirstOrDefault().Id != updateLogEquipmentViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }

            var existingLogEquipment1 = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByCode(updateLogEquipmentViewModel.Code);
            if (!existingLogEquipment1.IsNullOrEmpty())
            {
                if (existingLogEquipment1.FirstOrDefault().Id != updateLogEquipmentViewModel.Id)
                {
                    throw new Exception("Code đã tồn tại");
                }
            }

            var existingLogEquipment2 = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByEquipmentId(updateLogEquipmentViewModel.EquipmentId);
            if (!existingLogEquipment2.IsNullOrEmpty())
            {
                if (existingLogEquipment2.FirstOrDefault().Id != updateLogEquipmentViewModel.Id)
                {
                    throw new Exception("Thiết bị đã tồn tại");
                }
            }

            var mapper = _mapper.Map<LogEquipment>(logEquipment);
            mapper.EquipmentId = updateLogEquipmentViewModel.EquipmentId;
            mapper.UserAccountId = updateLogEquipmentViewModel.UserAccountId;
            mapper.Name = updateLogEquipmentViewModel.Name;
            mapper.Code = updateLogEquipmentViewModel.Code;
            mapper.Price = updateLogEquipmentViewModel.Price;
            mapper.Status = updateLogEquipmentViewModel.Status;
            mapper.WarrantyDate = updateLogEquipmentViewModel.WarrantyDate;
            mapper.WarrantyPeriod = updateLogEquipmentViewModel.WarrantyPeriod;
            mapper.PurchaseDate = updateLogEquipmentViewModel.PurchaseDate;
            mapper.RoomId = updateLogEquipmentViewModel.RoomId;
            _unitOfWork.LogEquipmentRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật bài học thất bại");
        }
    }
}
