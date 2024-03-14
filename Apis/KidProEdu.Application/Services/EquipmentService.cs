using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.CategoryEquipments;
using KidProEdu.Application.Validations.Equipments;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
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

        public async Task<bool> EquipmentManagement(EquipmentWithLogEquipmentViewModel equipmentWithLogEquipmentView)
        {
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentWithLogEquipmentView.Equipment.Id);
            var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }else{
                if (equipment.Status == StatusOfEquipment.Borrowed && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Borrowed)
                {
                    throw new Exception("Thiết bị đang được cho mượn");
                }
                else if(equipment.Status == StatusOfEquipment.Borrowed && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Repair)
                {
                    throw new Exception("Thiết bị đang được cho mượn nên không thể bảo dưỡng");
                }
                else if (equipment.Status == StatusOfEquipment.Repair && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Borrowed)
                {
                    throw new Exception("Thiết bị đang được bảo dưỡng nên không thể cho mượn");
                }
                else if (equipment.Status == StatusOfEquipment.Returned && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Returned)
                {
                    throw new Exception("Thiết bị đã được trả lại rồi");
                }
                else if (equipment.Status == StatusOfEquipment.Repair && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Repair)
                {
                    throw new Exception("Thiết bị đã được mang đi bảo dưỡng rồi");
                }
                else if (equipment.Status == StatusOfEquipment.Repair && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Returned && equipmentWithLogEquipmentView.Log.UserAccountId != logEquipment.UserAccountId)
                {
                    throw new Exception("Người mang thiết bị đi bảo dưỡng khác với người nhận lại thiết bị");
                }
                else if (equipment.Status == StatusOfEquipment.Borrowed && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Returned && equipmentWithLogEquipmentView.Log.UserAccountId != logEquipment.UserAccountId)
                {
                    throw new Exception("Người muợn thiết bị khác với người trả thiết bị");
                }

            }
            // Cập nhật trạng thái thiết bị
            var mapper = _mapper.Map<Equipment>(equipment);
            mapper.Status = equipmentWithLogEquipmentView.Equipment.Status;
            mapper.RoomId = equipmentWithLogEquipmentView.Equipment.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);

            var mapper2 = _mapper.Map<LogEquipment>(equipmentWithLogEquipmentView.Log);            
            mapper2.EquipmentId = equipment.Id;
            mapper2.UserAccountId = equipmentWithLogEquipmentView.Log.UserAccountId;
            mapper2.Name = equipment.Name;
            mapper2.Code = equipment.Code;
            mapper2.Price = equipment.Price;
            mapper2.Status = equipmentWithLogEquipmentView.Equipment.Status;
            mapper2.RepairDate = equipmentWithLogEquipmentView.Log.RepairDate;
            mapper2.BorrowedDate = equipmentWithLogEquipmentView.Log.BorrowedDate;
            mapper2.ReturnedDate = equipmentWithLogEquipmentView.Log.ReturnedDate;
            mapper2.ReturnedDealine = equipmentWithLogEquipmentView.Log.ReturnedDealine;
            mapper2.WarrantyPeriod = equipment.WarrantyPeriod;
            mapper2.PurchaseDate = equipment.PurchaseDate;
            mapper2.RoomId = equipmentWithLogEquipmentView.Equipment.RoomId;
            _unitOfWork.LogEquipmentRepository.AddAsync(mapper2);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thất bại");
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
