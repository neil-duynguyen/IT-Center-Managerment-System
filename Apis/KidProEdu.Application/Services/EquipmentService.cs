using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.CategoryEquipments;
using KidProEdu.Application.Validations.Equipments;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public EquipmentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateEquipment(CreateEquipmentViewModel createEquipmentViewModel)
        {
            var validator = new CreateEquipmentViewModelValidator();
            var validationResult = validator.Validate(createEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var equipment = await _unitOfWork.EquipmentRepository.GetEquipmentByName(createEquipmentViewModel.Name);
            if (!equipment.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var equipment1 = await _unitOfWork.EquipmentRepository.GetEquipmentByCode(createEquipmentViewModel.Code);
            if (!equipment1.IsNullOrEmpty())
            {
                throw new Exception("Code đã tồn tại");
            }

            var mapper = _mapper.Map<Equipment>(createEquipmentViewModel);
            await _unitOfWork.EquipmentRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo thiết bị thất bại");
        }

        public async Task<bool> DeleteEquipment(Guid id)
        {
            var result = await _unitOfWork.EquipmentRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy thiết bị");
            else
            {
                _unitOfWork.EquipmentRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa thiết bị thất bại");
            }
        }

        public async Task<Equipment> GetEquipmentById(Guid id)
        {
            var result = await _unitOfWork.EquipmentRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<List<Equipment>> GetEquipments()
        {
            var results = _unitOfWork.EquipmentRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return results;
        }

        public async Task<bool> UpdateEquipment(UpdateEquipmentViewModel updateEquipmentViewModel)
        {
            var validator = new UpdateEquipmentViewModelValidator();
            var validationResult = validator.Validate(updateEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(updateEquipmentViewModel.Id);
            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }

            var existingEquipment = await _unitOfWork.EquipmentRepository.GetEquipmentByName(updateEquipmentViewModel.Name);
            if (!existingEquipment.IsNullOrEmpty())
            {
                if (existingEquipment.FirstOrDefault().Id != equipment.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }

            var existingEquipment2 = await _unitOfWork.EquipmentRepository.GetEquipmentByCode(updateEquipmentViewModel.Code);
            if (!existingEquipment2.IsNullOrEmpty())
            {
                if (existingEquipment2.FirstOrDefault().Id != equipment.Id)
                {
                    throw new Exception("Code đã tồn tại");
                }
            }

            equipment.Name = updateEquipmentViewModel.Name;
            equipment.CategoryEquipmentId = updateEquipmentViewModel.CategoryEquipmentId;
            equipment.Status = updateEquipmentViewModel.Status;
            equipment.Price = updateEquipmentViewModel.Price;
            equipment.WarrantyDate = updateEquipmentViewModel.WarrantyDate;
            equipment.Code = updateEquipmentViewModel.Code;
            equipment.RoomId = updateEquipmentViewModel.RoomId;

            _unitOfWork.EquipmentRepository.Update(equipment);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thiết bị thất bại");
        }
    }
}
