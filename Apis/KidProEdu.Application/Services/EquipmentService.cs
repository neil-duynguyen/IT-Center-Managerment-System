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

        public async Task<bool> EquipmentBorrowedManagement(EquipmentWithLogEquipmentBorrowedViewModel equipmentWithLogEquipmentBorrowedViewModel)
        {
            var validator = new LogEquipmentBorrowedManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentWithLogEquipmentBorrowedViewModel.Log);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentWithLogEquipmentBorrowedViewModel.Equipment.Id);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else
            {
                if (equipment.Status == StatusOfEquipment.Borrowed)
                {
                    throw new Exception("Thiết bị đang được cho mượn");
                }
                else if (equipment.Status == StatusOfEquipment.Repair)
                {
                    throw new Exception("Thiết bị đang được bảo dưỡng nên không thể cho mượn");
                }
            }
            // Cập nhật trạng thái thiết bị

            equipment.Status = StatusOfEquipment.Borrowed;
            equipment.RoomId = equipmentWithLogEquipmentBorrowedViewModel.Equipment.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);

            var mapper2 = _mapper.Map<LogEquipment>(equipmentWithLogEquipmentBorrowedViewModel.Log);
            mapper2.EquipmentId = equipment.Id;
            mapper2.UserAccountId = equipmentWithLogEquipmentBorrowedViewModel.Log.UserAccountId;
            mapper2.Name = equipment.Name;
            mapper2.Code = equipment.Code;
            mapper2.Price = equipment.Price;
            mapper2.Status = StatusOfEquipment.Borrowed;
            mapper2.RepairDate = null;
            mapper2.BorrowedDate = DateTime.Now;
            mapper2.ReturnedDate = null;
            mapper2.ReturnedDealine = equipmentWithLogEquipmentBorrowedViewModel.Log.ReturnedDealine;
            mapper2.WarrantyPeriod = equipment.WarrantyPeriod;
            mapper2.PurchaseDate = equipment.PurchaseDate;
            mapper2.RoomId = equipmentWithLogEquipmentBorrowedViewModel.Equipment.RoomId;
            await _unitOfWork.LogEquipmentRepository.AddAsync(mapper2);
            var result = await _unitOfWork.SaveChangeAsync();
            var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)equipmentWithLogEquipmentBorrowedViewModel.Log.UserAccountId);
            if (result > 0)
            {
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
                    "         Ngày hẹn trả: " + equipmentWithLogEquipmentBorrowedViewModel.Log.ReturnedDealine + "\n" +
                    "Vui lòng trả thiết bị đúng trong thời hạn đã hẹn, xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                return true;
            }
            else
            {
                throw new Exception("Mượn thiết bị thất bại");
            }
        }

        public async Task<bool> EquipmentRepairManagement(EquipmentWithLogEquipmentRepairViewModel equipmentWithLogEquipmentRepairViewModel)
        {
            var validator = new LogEquipmentRepairManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentWithLogEquipmentRepairViewModel.Log);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentWithLogEquipmentRepairViewModel.Equipment.Id);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else
            {
                if (equipment.Status == StatusOfEquipment.Borrowed)
                {
                    throw new Exception("Thiết bị đang được cho mượn không thể mang đi bảo dưỡng");
                }
                else if (equipment.Status == StatusOfEquipment.Repair)
                {
                    throw new Exception("Thiết bị đang được bảo dưỡng rồi");
                }
            }
            // Cập nhật trạng thái thiết bị
            equipment.Status = StatusOfEquipment.Repair;
            equipment.RoomId = equipmentWithLogEquipmentRepairViewModel.Equipment.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);

            var mapper2 = _mapper.Map<LogEquipment>(equipmentWithLogEquipmentRepairViewModel.Log);
            mapper2.EquipmentId = equipment.Id;
            mapper2.UserAccountId = equipmentWithLogEquipmentRepairViewModel.Log.UserAccountId;
            mapper2.Name = equipment.Name;
            mapper2.Code = equipment.Code;
            mapper2.Price = equipment.Price;
            mapper2.Status = StatusOfEquipment.Repair;
            mapper2.RepairDate = DateTime.Now;
            mapper2.BorrowedDate = null;
            mapper2.ReturnedDate = null;
            mapper2.ReturnedDealine = null;
            mapper2.WarrantyPeriod = equipment.WarrantyPeriod;
            mapper2.PurchaseDate = equipment.PurchaseDate;
            mapper2.RoomId = equipmentWithLogEquipmentRepairViewModel.Equipment.RoomId;
            await _unitOfWork.LogEquipmentRepository.AddAsync(mapper2);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Mang thiết bị đi sửa thất bại");
        }

        public async Task<bool> EquipmentReturnedManagement(EquipmentWithLogEquipmentReturnedViewModel equipmentWithLogEquipmentReturnedViewModel)
        {
            var validator = new LogEquipmentReturnedManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentWithLogEquipmentReturnedViewModel.Log);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentWithLogEquipmentReturnedViewModel.Equipment.Id);
            var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else
            {
                if (equipment.Status == StatusOfEquipment.Returned)
                {
                    throw new Exception("Thiết bị đã được trả rồi");
                }
                /*else if (equipment.Status == StatusOfEquipment.Repair && equipmentWithLogEquipmentReturnedViewModel.Log.UserAccountId != logEquipment.UserAccountId)
                {
                    throw new Exception("Người mang thiết bị đi bảo dưỡng khác với người trả thiết bị");
                }*/
                else if (equipment.Status == StatusOfEquipment.Borrowed && equipmentWithLogEquipmentReturnedViewModel.Log.UserAccountId != logEquipment.UserAccountId)
                {
                    throw new Exception("Người mượn thiết bị khác với người trả thiết bị");
                }
            }
            // Cập nhật trạng thái thiết bị
            equipment.Status = StatusOfEquipment.Returned;
            equipment.RoomId = equipmentWithLogEquipmentReturnedViewModel.Equipment.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);

            var log = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result
                .Where(x => x.Status == StatusOfEquipment.Borrowed).OrderByDescending(x => x.CreationDate).FirstOrDefault();

            var mapper2 = _mapper.Map<LogEquipment>(equipmentWithLogEquipmentReturnedViewModel.Log);
            mapper2.EquipmentId = equipment.Id;
            mapper2.UserAccountId = equipmentWithLogEquipmentReturnedViewModel.Log.UserAccountId;
            mapper2.Name = equipment.Name;
            mapper2.Code = equipment.Code;
            mapper2.Price = equipment.Price;
            mapper2.Status = StatusOfEquipment.Returned;
            mapper2.RepairDate = null;
            mapper2.BorrowedDate = null;
            mapper2.ReturnedDate = DateTime.Now;
            mapper2.ReturnedDealine = log.ReturnedDealine;
            mapper2.WarrantyPeriod = equipment.WarrantyPeriod;
            mapper2.PurchaseDate = equipment.PurchaseDate;
            mapper2.RoomId = equipmentWithLogEquipmentReturnedViewModel.Equipment.RoomId;
            await _unitOfWork.LogEquipmentRepository.AddAsync(mapper2);
            var result = await _unitOfWork.SaveChangeAsync();
            var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)equipmentWithLogEquipmentReturnedViewModel.Log.UserAccountId);
            if (result > 0)
            {
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
                    "         Ngày trả: " + mapper2.ReturnedDate + "\n" +
                    "Xác nhận yêu cầu trả thành công, xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                return true;
            }
            else
            {
                throw new Exception("Trả thiết bị thất bại");
            }
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

        public async Task<EquipmentViewModel2> GetEquipmentById(Guid id)
        {
            var result = await _unitOfWork.EquipmentRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<EquipmentViewModel2>(result);
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
