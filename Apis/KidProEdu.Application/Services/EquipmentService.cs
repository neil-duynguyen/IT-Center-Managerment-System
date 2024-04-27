using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.CategoryEquipments;
using KidProEdu.Application.Validations.Equipments;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
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
using ZXing;

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
            mapper.Status = StatusOfEquipment.Returned;
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

        public async Task<bool> EquipmentBorrowedManagement(EquipmentBorrowedManagementViewModel equipmentBorrowedManagementViewModel)
        {
            var validator = new EquipmentBorrowedManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentBorrowedManagementViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentBorrowedManagementViewModel.EquipmentId);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else if(equipment.Status == StatusOfEquipment.Borrowed)
            {
                throw new Exception("Thiết bị đang được cho mượn");
            }
            else if (equipment.Status == StatusOfEquipment.Repair)
            {
                throw new Exception("Thiết bị đang được bảo dưỡng nên không thể cho mượn");
            }

            // Cập nhật trạng thái thiết bị

            equipment.Status = StatusOfEquipment.Borrowed;
            equipment.RoomId = equipmentBorrowedManagementViewModel.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);
            var result = await _unitOfWork.SaveChangeAsync();
            if(result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = equipment.Id;
                logEquipment.UserAccountId = equipmentBorrowedManagementViewModel.UserAccountId;
                logEquipment.Name = equipment.Name;
                logEquipment.Code = equipment.Code;
                logEquipment.Price = equipment.Price;
                logEquipment.Status = StatusOfEquipment.Borrowed;
                logEquipment.RepairDate = null;
                logEquipment.BorrowedDate = _currentTime.GetCurrentTime().Date;
                logEquipment.ReturnedDate = null;
                logEquipment.ReturnedDealine = equipmentBorrowedManagementViewModel.ReturnedDealine;
                logEquipment.WarrantyPeriod = equipment.WarrantyPeriod;
                logEquipment.PurchaseDate = equipment.PurchaseDate;
                logEquipment.RoomId = equipmentBorrowedManagementViewModel.RoomId;
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)equipmentBorrowedManagementViewModel.UserAccountId);
                    await SendEmailUtil.SendEmail(teacher.Email, "Xác nhận yêu cầu mượn thiết bị",
                    "KidProEdu thông báo, \n\n" +
                    "Yêu cầu mượn thiết bị của bạn đã được xác nhận, \n" +
                    "   Thông tin:, \n" +
                    "         Người mượn: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + equipment.Code + "\n" +
                    "         Tên thiết bị: " + equipment.Name + "\n" +
                    //"         Loại thiết bị: " + equipment.CategoryEquipment.Name + "\n" +
                    "         Ngày hẹn trả: " + equipmentBorrowedManagementViewModel.ReturnedDealine + "\n" +
                    "Vui lòng trả thiết bị đúng trong thời hạn đã hẹn, xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> EquipmentRepairManagement(EquipmentRepairManagementViewModel equipmentRepairManagementViewModel)
        {
            var validator = new EquipmentRepairManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentRepairManagementViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentRepairManagementViewModel.EquipmentId);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else if (equipment.Status == StatusOfEquipment.Borrowed)
            {
                throw new Exception("Thiết bị đang được cho mượn không thể mang đi bảo dưỡng");
            }
            else if (equipment.Status == StatusOfEquipment.Repair)
            {
                throw new Exception("Thiết bị đang được bảo dưỡng rồi");
            }

            // Cập nhật trạng thái thiết bị
            equipment.Status = StatusOfEquipment.Repair;
            equipment.RoomId = equipmentRepairManagementViewModel.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);

            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = equipment.Id;
                logEquipment.UserAccountId = equipmentRepairManagementViewModel.UserAccountId;
                logEquipment.Name = equipment.Name;
                logEquipment.Code = equipment.Code;
                logEquipment.Price = equipment.Price;
                logEquipment.Status = StatusOfEquipment.Repair;
                logEquipment.RepairDate = _currentTime.GetCurrentTime().Date;
                logEquipment.BorrowedDate = null;
                logEquipment.ReturnedDate = null;
                logEquipment.ReturnedDealine = null;
                logEquipment.WarrantyPeriod = equipment.WarrantyPeriod;
                logEquipment.PurchaseDate = equipment.PurchaseDate;
                logEquipment.RoomId = equipmentRepairManagementViewModel.RoomId;
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {                  
                    return true;
                }
            }
            return false;        
        }

        public async Task<bool> EquipmentReturnedManagement(EquipmentReturnedManagementViewModel equipmentReturnedManagementViewModel)
        {
            var validator = new EquipmentReturnedManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentReturnedManagementViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentReturnedManagementViewModel.EquipmentId);          
            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else if (equipment.Status == StatusOfEquipment.Returned)
            {
                throw new Exception("Thiết bị đã được trả rồi");
            }
            // Cập nhật trạng thái thiết bị
            equipment.Status = StatusOfEquipment.Returned;
            equipment.RoomId = equipmentReturnedManagementViewModel.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = equipment.Id;
                logEquipment.UserAccountId = equipmentReturnedManagementViewModel.UserAccountId;
                logEquipment.Name = equipment.Name;
                logEquipment.Code = equipment.Code;
                logEquipment.Price = equipment.Price;
                logEquipment.Status = StatusOfEquipment.Returned;
                logEquipment.RepairDate = null;
                logEquipment.BorrowedDate = null;
                logEquipment.ReturnedDate = _currentTime.GetCurrentTime().Date;
                logEquipment.ReturnedDealine = null;
                logEquipment.WarrantyPeriod = equipment.WarrantyPeriod;
                logEquipment.PurchaseDate = equipment.PurchaseDate;
                logEquipment.RoomId = equipmentReturnedManagementViewModel.RoomId;
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)equipmentReturnedManagementViewModel.UserAccountId);
                    await SendEmailUtil.SendEmail(teacher.Email, "Xác nhận trả thiết bị",
                    "KidProEdu thông báo, \n\n" +
                    "Yêu cầu trả thiết bị  \n" +
                    "   Thông tin:, \n" +
                    "         Người trả: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + equipment.Code + "\n" +
                    "         Tên thiết bị: " + equipment.Name + "\n" +
                    //"         Loại thiết bị: " + equipment.CategoryEquipment.Name + "\n" +
                    "         Ngày trả: " + logEquipment.ReturnedDate + "\n" +
                    "Xác nhận yêu cầu trả thành công, xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                    return true;
                }
            }
            return false;           
        }

        /*public async Task<bool> EquipmentManagement(EquipmentWithLogEquipmentBorrowedViewModel equipmentWithLogEquipmentView)
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
        }*/

        public async Task<EquipmentByIdViewModel> GetEquipmentById(Guid id)
        {
            var result = await _unitOfWork.EquipmentRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<EquipmentByIdViewModel>(result);
            return mapper;

        }

        public async Task<List<EquipmentViewModel>> GetEquipments()
        {
            var results = _unitOfWork.EquipmentRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
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

            equipment.CategoryEquipmentId = updateEquipmentViewModel.CategoryEquipmentId;
            equipment.RoomId = updateEquipmentViewModel.RoomId;
            equipment.Name = updateEquipmentViewModel.Name;
            equipment.Price = updateEquipmentViewModel.Price;
            equipment.Status = updateEquipmentViewModel.Status;
            equipment.WarrantyPeriod = updateEquipmentViewModel.WarrantyPeriod;
            equipment.PurchaseDate = updateEquipmentViewModel.PurchaseDate;
            _unitOfWork.EquipmentRepository.Update(equipment);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thiết bị thất bại");
        }

        public async Task AutoCheckReturn()
        {
            var equipments = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByReturnDeadline(_currentTime.GetCurrentTime().AddDays(1));
            foreach (var item in equipments)
            {
                var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)item.UserAccountId);
                await SendEmailUtil.SendEmail(teacher.Email, "Nhắc nhở sắp tới hạn trả thiết bị",
                    "KidPro thông báo, \n\n" +
                    "Nhắc nhở sắp tới hạn trả thiết bị \n" +
                    "  Thông tin: \n" +
                    "         Người mượn: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + item.Code + "\n" +
                    "         Tên thiết bị: " + item.Name + "\n" +
                    "         Ngày hẹn trả: " + item.ReturnedDealine + "\n" +
                    "Vui lòng bỏ qua email nếu bạn đã trả thiết bị rồi. \n\n" +
                    "Xin cảm ơn, \n" +
                    "KidPro Education!");
            }
        }

        public async Task<List<EquipmentViewModel>> GetEquipmentByStatus(StatusOfEquipment status)
        {
            var results = await _unitOfWork.EquipmentRepository.GetListEquipmentByStatus(status);
            var mapper = _mapper.Map<List<EquipmentViewModel>>(results);
            return mapper;
        }
    }
}
