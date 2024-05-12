using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.CategoryEquipments;
using KidProEdu.Application.Validations.Equipments;
using KidProEdu.Application.Validations.Locations;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
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

    public class CategoryEquipmentService : ICategoryEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly QRCodeUtility _qrCodeUtility;

        public CategoryEquipmentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, QRCodeUtility qrCodeUtility)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _qrCodeUtility = qrCodeUtility;
        }

        public async Task<bool> BorrowAutoCategoryEquipment(BorrowAutoCategoryEquipmentViewModel borrowAutoCategoryEquipmentViewModel)
        {
            var validator = new BorrowAutoCategoryEquipmentViewModelValidator();
            var validationResult = validator.Validate(borrowAutoCategoryEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(borrowAutoCategoryEquipmentViewModel.CategoryEquipmentId);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (categoryEquipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }

            if (categoryEquipment.Quantity <= 0)
            {
                throw new Exception("Thiết bị trong kho đã được mượn hết");
            }

            if (categoryEquipment.Quantity < borrowAutoCategoryEquipmentViewModel.Quantity)
            {
                throw new Exception("Thiết bị trong kho đã không đủ số lượng bạn yêu cầu");
            }

            // Cập nhật trạng thái thiết bị

            categoryEquipment.Quantity = categoryEquipment.Quantity - borrowAutoCategoryEquipmentViewModel.Quantity;
            _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = null;
                logEquipment.UserAccountId = borrowAutoCategoryEquipmentViewModel.UserAccountId;
                logEquipment.Name = categoryEquipment.Name;
                logEquipment.Code = categoryEquipment.Code;
                logEquipment.Price = null;
                logEquipment.Status = StatusOfEquipment.Borrowed;
                logEquipment.RepairDate = null;
                logEquipment.BorrowedDate = _currentTime.GetCurrentTime().Date;
                logEquipment.ReturnedDate = null;
                logEquipment.ReturnedDealine = borrowAutoCategoryEquipmentViewModel.ReturnedDealine;
                logEquipment.WarrantyPeriod = null;
                logEquipment.PurchaseDate = null;
                logEquipment.RoomId = null;
                logEquipment.CategoryEquipmentId = borrowAutoCategoryEquipmentViewModel.CategoryEquipmentId;
                logEquipment.Quantity = borrowAutoCategoryEquipmentViewModel.Quantity;
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)borrowAutoCategoryEquipmentViewModel.UserAccountId);
                    await SendEmailUtil.SendEmail(teacher.Email, "Xác nhận yêu cầu mượn thiết bị",
                    "KidProEdu thông báo, \n\n" +
                    "Yêu cầu mượn thiết bị của bạn đã được xác nhận, \n" +
                    "   Thông tin:, \n" +
                    "         Người mượn: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + equipment.Code + "\n" +
                    "         Tên thiết bị: " + categoryEquipment.Name + "\n" +
                    "         Số lượng: " + borrowAutoCategoryEquipmentViewModel.Quantity + "\n" +
                    //"         Loại thiết bị: " + equipment.CategoryEquipment.Name + "\n" +
                    "         Ngày hẹn trả: " + borrowAutoCategoryEquipmentViewModel.ReturnedDealine + "\n" +
                    //"Nhân viên sẽ đưa thiết bị đến phòng" + borrowCategoryEquipmentViewModel.RoomId + "\n" +
                    "Vui lòng trả thiết bị đúng trong thời hạn đã hẹn, xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> BorrowCategoryEquipment(BorrowCategoryEquipmentViewModel borrowCategoryEquipmentViewModel)
        {
            var validator = new BorrowCategoryEquipmentViewModelValidator();
            var validationResult = validator.Validate(borrowCategoryEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(borrowCategoryEquipmentViewModel.CategoryEquipmentId);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (categoryEquipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }

            if (categoryEquipment.Quantity <= 0)
            {
                throw new Exception("Thiết bị trong kho đã được mượn hết");
            }

            if (categoryEquipment.Quantity < borrowCategoryEquipmentViewModel.Quantity)
            {
                throw new Exception("Thiết bị trong kho đã không đủ số lượng bạn yêu cầu");
            }

            // Cập nhật trạng thái thiết bị

            categoryEquipment.Quantity = categoryEquipment.Quantity - borrowCategoryEquipmentViewModel.Quantity;
            _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = null;
                logEquipment.UserAccountId = borrowCategoryEquipmentViewModel.UserAccountId;
                logEquipment.Name = categoryEquipment.Name;
                logEquipment.Code = categoryEquipment.Code;
                logEquipment.Price = null;
                logEquipment.Status = StatusOfEquipment.Borrowed;
                logEquipment.RepairDate = null;
                logEquipment.BorrowedDate = _currentTime.GetCurrentTime().Date;
                logEquipment.ReturnedDate = null;
                logEquipment.ReturnedDealine = borrowCategoryEquipmentViewModel.ReturnedDealine;
                logEquipment.WarrantyPeriod = null;
                logEquipment.PurchaseDate = null;
                logEquipment.RoomId = borrowCategoryEquipmentViewModel.RoomId;
                logEquipment.CategoryEquipmentId = borrowCategoryEquipmentViewModel.CategoryEquipmentId;
                logEquipment.Quantity = borrowCategoryEquipmentViewModel.Quantity;
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)borrowCategoryEquipmentViewModel.UserAccountId);
                    await SendEmailUtil.SendEmail(teacher.Email, "Xác nhận yêu cầu mượn thiết bị",
                    "KidProEdu thông báo, \n\n" +
                    "Yêu cầu mượn thiết bị của bạn đã được xác nhận, \n" +
                    "   Thông tin:, \n" +
                    "         Người mượn: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + equipment.Code + "\n" +
                    "         Tên thiết bị: " + categoryEquipment.Name + "\n" +
                    "         Số lượng: " + borrowCategoryEquipmentViewModel.Quantity + "\n" +
                    //"         Loại thiết bị: " + equipment.CategoryEquipment.Name + "\n" +
                    "         Ngày hẹn trả: " + borrowCategoryEquipmentViewModel.ReturnedDealine + "\n" +
                    "Nhân viên sẽ đưa thiết bị đến phòng" + borrowCategoryEquipmentViewModel.RoomId + "\n" +
                    "Vui lòng trả thiết bị đúng trong thời hạn đã hẹn, xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                    return true;
                }
            }

            return false;

        }


        public async Task<bool> CreateCategoryEquipment(CreateCategoryEquipmentViewModel createCategoryEquipmentViewModel)
        {
            var validator = new CreateCategoryEquipmentViewModelValidator();
            var validationResult = validator.Validate(createCategoryEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetCategoryEquipmentByName(createCategoryEquipmentViewModel.Name);
            if (categoryEquipment != null)
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<CategoryEquipment>(createCategoryEquipmentViewModel);
            await _unitOfWork.CategoryEquipmentRepository.AddAsync(mapper);
            mapper.Code = _qrCodeUtility.GenerateQRCode($"{mapper.Id}");
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo danh mục thiết bị thất bại");
        }
       

        public async Task<bool> DeleteCategoryEquipment(Guid id)
        {
            var result = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy danh mục thiết bị");
            else
            {
                _unitOfWork.CategoryEquipmentRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa danh mục thiết bị thất bại");
            }
        }

        public async Task<CategoryEquipmentViewModel> GetCategoryEquipmentById(Guid id)
        {
            var result = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<CategoryEquipmentViewModel>(result);
            return mapper;
        }

        public async Task<List<CategoryEquipmentViewModel>> GetCategoryEquipments()
        {
            var results = _unitOfWork.CategoryEquipmentRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mappers = _mapper.Map<List<CategoryEquipmentViewModel>>(results);
            return mappers;
        }

        public async Task<bool> ReturnCategoryEquipment(ReturnCategoryEquipmentViewModel returnCategoryEquipmentViewModel)
        {
            var validator = new ReturnCategoryEquipmentViewModelValidator();
            var validationResult = validator.Validate(returnCategoryEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(returnCategoryEquipmentViewModel.CategoryEquipmentId);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (categoryEquipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            // Cập nhật trạng thái thiết bị

            categoryEquipment.Quantity = categoryEquipment.Quantity + returnCategoryEquipmentViewModel.Quantity;
            _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = null;
                logEquipment.UserAccountId = returnCategoryEquipmentViewModel.UserAccountId;
                logEquipment.Name = categoryEquipment.Name;
                logEquipment.Code = categoryEquipment.Code;
                logEquipment.Price = null;
                logEquipment.Status = StatusOfEquipment.Returned;
                logEquipment.RepairDate = null;
                logEquipment.BorrowedDate = null;
                logEquipment.ReturnedDate = _currentTime.GetCurrentTime().Date;
                logEquipment.ReturnedDealine = null;
                logEquipment.WarrantyPeriod = null;
                logEquipment.PurchaseDate = null;
                logEquipment.RoomId = null;
                logEquipment.CategoryEquipmentId = returnCategoryEquipmentViewModel.CategoryEquipmentId;
                logEquipment.Quantity = returnCategoryEquipmentViewModel.Quantity;
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)returnCategoryEquipmentViewModel.UserAccountId);
                    await SendEmailUtil.SendEmail(teacher.Email, "Xác nhận yêu cầu trả thiết bị",
                    "KidProEdu thông báo, \n\n" +
                    "Yêu cầu trả thiết bị của bạn đã được xác nhận, \n" +
                    "   Thông tin:, \n" +
                    "         Người trả: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + equipment.Code + "\n" +
                    "         Tên thiết bị: " + categoryEquipment.Name + "\n" +
                    "         Số lượng: " + returnCategoryEquipmentViewModel.Quantity + "\n" +
                    //"         Loại thiết bị: " + equipment.CategoryEquipment.Name + "\n" +
                    "         Ngày trả: " + _currentTime.GetCurrentTime().Date + "\n" +
                    //"Nhân viên sẽ đưa thiết bị đến phòng" + borrowCategoryEquipmentViewModel.RoomId + "\n" +
                    "Xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> UpdateCategoryEquipment(UpdateCategoryEquipmentViewModel updateCategoryEquipmentViewModel)
        {
            var validator = new UpdateCategoryEquipmentViewModelValidator();
            var validationResult = validator.Validate(updateCategoryEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(updateCategoryEquipmentViewModel.Id);
            if (categoryEquipment == null)
            {
                throw new Exception("Không tìm thấy danh mục thiết bị");
            }

            var existingCategoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetCategoryEquipmentByName(updateCategoryEquipmentViewModel.Name);
            if (existingCategoryEquipment != null)
            {
                if (existingCategoryEquipment.Id != categoryEquipment.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }

            categoryEquipment.Name = updateCategoryEquipmentViewModel.Name;
            categoryEquipment.Description = updateCategoryEquipmentViewModel.Description;
            categoryEquipment.TypeCategoryEquipment = updateCategoryEquipmentViewModel.TypeCategoryEquipment;
            categoryEquipment.Id = updateCategoryEquipmentViewModel.Id;
            categoryEquipment.Quantity = updateCategoryEquipmentViewModel.Quantity;

            _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật danh mục thiết bị thất bại");
        }
    }
}
