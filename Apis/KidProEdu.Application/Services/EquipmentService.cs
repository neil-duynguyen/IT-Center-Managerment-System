using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
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
        private readonly QRCodeUtility _qrCodeUtility;

        public EquipmentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, QRCodeUtility qrCodeUtility)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _qrCodeUtility = qrCodeUtility;
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

            var mapper = _mapper.Map<Equipment>(createEquipmentViewModel);
            await _unitOfWork.EquipmentRepository.AddAsync(mapper);
            mapper.Code = _qrCodeUtility.GenerateQRCode($"{mapper.Id}");
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

        public async Task<EquipmentViewModel> GetEquipmentById(Guid id)
        {
            var result = await _unitOfWork.EquipmentRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<EquipmentViewModel>(result);
            return mapper;

        }

        public async Task<List<EquipmentViewModel>> GetEquipments()
        {
            var results =  _unitOfWork.EquipmentRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<EquipmentViewModel>>(results);
            return mapper;
        }

        public async Task<List<EquipmentViewModel>> GetListEquipmentByName(string name)
        {
            var results = _unitOfWork.EquipmentRepository.GetListEquipmentByName(name).Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<EquipmentViewModel>>(results);
            return mapper;
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

            var mapper = _mapper.Map<Equipment>(equipment);
            mapper.CategoryEquipmentId = updateEquipmentViewModel.CategoryEquipmentId;
            mapper.RoomId = updateEquipmentViewModel.RoomId;
            mapper.Name = updateEquipmentViewModel.Name;
            mapper.Price = updateEquipmentViewModel.Price;
            mapper.Status = updateEquipmentViewModel.Status;
            mapper.WarrantyPeriod = updateEquipmentViewModel.WarrantyPeriod;
            mapper.PurchaseDate = updateEquipmentViewModel.PurchaseDate;
            _unitOfWork.EquipmentRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thiết bị thất bại");
        }
    }
}
