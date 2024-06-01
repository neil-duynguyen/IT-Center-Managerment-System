using AutoMapper;
using FluentValidation;
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
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KidProEdu.Application.Services
{

    public class CategoryEquipmentService : ICategoryEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly QRCodeUtility _qrCodeUtility;
        private readonly IEquipmentService _equipmentService;

        public CategoryEquipmentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, QRCodeUtility qrCodeUtility, IEquipmentService equipmentService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _qrCodeUtility = qrCodeUtility;
            _equipmentService = equipmentService;
        }

        public async Task<bool> BorrowForGoHomeCategoryEquipment(List<BorrowForGoHomeCategoryEquipmentViewModel> borrowForGoHomeCategoryEquipmentViewModels)
        {
            var validator = new BorrowForGoHomeCategoryEquipmentViewModelValidator();
            var errorMessages = new List<string>();
            foreach (var viewModel in borrowForGoHomeCategoryEquipmentViewModels)
            {
                var validationResult = validator.Validate(viewModel);
                if (!validationResult.IsValid)
                {
                    errorMessages.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
                }

                var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(viewModel.CategoryEquipmentId);
                if (categoryEquipment == null)
                {
                    errorMessages.Add($"Không tìm thấy thiết bị có category id là: {viewModel.CategoryEquipmentId}");
                }


                if (categoryEquipment.Quantity < viewModel.Quantity)
                {
                    errorMessages.Add($"Thiết bị {categoryEquipment.Name} trong kho đã không đủ số lượng bạn yêu cầu");
                }

                if(viewModel.Quantity > 5)
                {
                    throw new Exception("Bạn không được mượn quá 5 thiết bị");
                }

                if (viewModel.Quantity < 1)
                {
                    throw new Exception("Bạn không được nhập số lượng nhỏ hơn 0");
                }

                categoryEquipment.Quantity -= viewModel.Quantity;
                _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);
                var logEquipment = new LogEquipment
                {
                    EquipmentId = null,
                    UserAccountId = viewModel.UserAccountId,
                    Name = categoryEquipment.Name,
                    Code = categoryEquipment.Code,
                    Price = null,
                    Status = StatusOfEquipment.Borrowed,
                    RepairDate = null,
                    BorrowedDate = _currentTime.GetCurrentTime(),
                    ReturnedDate = null,
                    ReturnedDealine = viewModel.ReturnedDealine,
                    WarrantyPeriod = null,
                    PurchaseDate = null,
                    RoomId = null,
                    CategoryEquipmentId = viewModel.CategoryEquipmentId,
                    Quantity = viewModel.Quantity,
                    Note = null,
                    LogType = LogType.AtHome,
                };
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
            }
            if (errorMessages.Any())
            {
                return false;
            }
            else
            {
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
        }

        //Function update số lượng equipment trong kho
        public async Task UpdateQuantityEquipment(UpdateQuantityCategoryEquipment updateQuantityCategory)
        {

            //check Teacher send request
            var getRequest = await _unitOfWork.RequestRepository.GetAllAsync();
            var checkRequest = getRequest.FirstOrDefault(x =>
                                                            x.CreatedBy == updateQuantityCategory.TeacherId &&
                                                            x.Status == StatusOfRequest.Pending &&
                                                            x.RequestType == "Equipment" &&
                                                            DateOnly.FromDateTime(x.CreationDate) == DateOnly.FromDateTime(_currentTime.GetCurrentTime()) &&
                                                            !x.IsDeleted);
            if (checkRequest != null)
            {
                checkRequest.Status = StatusOfRequest.Approved;
                _unitOfWork.RequestRepository.Update(checkRequest);
                await _unitOfWork.SaveChangeAsync();
            }
            else
            {
                throw new Exception("Giáo viên chưa gửi yêu cầu mượn thiết bị.");
            }

            var getEquipment = await _equipmentService.GetEquipmentByProgress(updateQuantityCategory.ClassId, updateQuantityCategory.Progress);

            var listEquipment = new List<BorrowAutoCategoryEquipmentViewModel>();

            if (getEquipment.Count != 0)
            {
                foreach (var item in getEquipment)
                {
                    listEquipment.Add(new BorrowAutoCategoryEquipmentViewModel() { CategoryEquipmentId = item.Id, Quantity = (int)item.Quantity, UserAccountId = updateQuantityCategory.TeacherId });
                }

                await BorrowCategoryEquipment(listEquipment);
            }
            else
            {
                throw new Exception("Cho mượn thất bại.");
            }
        }

        public async Task<bool> BorrowCategoryEquipment(List<BorrowAutoCategoryEquipmentViewModel> borrowCategoryEquipmentViewModels)
        {
            var validator = new BorrowAutoCategoryEquipmentViewModelValidator();
            var errorMessages = new List<string>();
            foreach (var viewModel in borrowCategoryEquipmentViewModels)
            {
                var validationResult = validator.Validate(viewModel);
                if (!validationResult.IsValid)
                {
                    errorMessages.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
                }

                var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(viewModel.CategoryEquipmentId);
                if (categoryEquipment == null)
                {
                    errorMessages.Add($"Không tìm thấy thiết bị có category id là: {viewModel.CategoryEquipmentId}");
                }


                if (categoryEquipment.Quantity < viewModel.Quantity)
                {
                    errorMessages.Add($"Thiết bị {categoryEquipment.Name} trong kho đã không đủ số lượng bạn yêu cầu");
                }

                if (viewModel.Quantity < 1)
                {
                    throw new Exception("Bạn không được nhập số lượng nhỏ hơn 0");
                }

                categoryEquipment.Quantity -= viewModel.Quantity;
                _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);
                var logEquipment = new LogEquipment
                {
                    EquipmentId = null,
                    UserAccountId = viewModel.UserAccountId,
                    Name = categoryEquipment.Name,
                    Code = categoryEquipment.Code,
                    Price = null,
                    Status = StatusOfEquipment.Borrowed,
                    RepairDate = null,
                    BorrowedDate = _currentTime.GetCurrentTime(),
                    ReturnedDate = null,
                    ReturnedDealine = _currentTime.GetCurrentTime(),
                    WarrantyPeriod = null,
                    PurchaseDate = null,
                    RoomId = null,
                    CategoryEquipmentId = viewModel.CategoryEquipmentId,
                    Quantity = viewModel.Quantity,
                    Note = null,
                    LogType = LogType.AtClass
                };
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
            }
            if (errorMessages.Any())
            {
                return false;
            }
            else
            {
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
        }

        public async Task<bool> BorrowWithStaffCategoryEquipment(List<BorrowCategoryEquipmentViewModel> borrowCategoryEquipmentViewModels)
        {
            var validator = new BorrowCategoryEquipmentViewModelValidator();
            var errorMessages = new List<string>();
            foreach (var viewModel in borrowCategoryEquipmentViewModels)
            {
                var validationResult = validator.Validate(viewModel);
                if (!validationResult.IsValid)
                {
                    errorMessages.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
                }

                var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(viewModel.CategoryEquipmentId);
                if (categoryEquipment == null)
                {
                    errorMessages.Add($"Không tìm thấy thiết bị có category id là: {viewModel.CategoryEquipmentId}");
                }


                if (categoryEquipment.Quantity < viewModel.Quantity)
                {
                    errorMessages.Add($"Thiết bị {categoryEquipment.Name} trong kho đã không đủ số lượng bạn yêu cầu");
                }

                categoryEquipment.Quantity -= viewModel.Quantity;
                _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);
                var logEquipment = new LogEquipment
                {
                    EquipmentId = null,
                    UserAccountId = viewModel.UserAccountId,
                    Name = categoryEquipment.Name,
                    Code = categoryEquipment.Code,
                    Price = null,
                    Status = StatusOfEquipment.Borrowed,
                    RepairDate = null,
                    BorrowedDate = _currentTime.GetCurrentTime(),
                    ReturnedDate = null,
                    ReturnedDealine = _currentTime.GetCurrentTime(),
                    WarrantyPeriod = null,
                    PurchaseDate = null,
                    RoomId = viewModel.RoomId,
                    CategoryEquipmentId = viewModel.CategoryEquipmentId,
                    Quantity = viewModel.Quantity,
                    Note = null,
                    LogType = LogType.AtClass
                };
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
            }
            if (errorMessages.Any())
            {
                return false;
            }
            else
            {
                await _unitOfWork.SaveChangeAsync();
                return true;
            }

        }

        public async Task<bool> ReturnCategoryEquipment(List<ReturnCategoryEquipmentViewModel> returnCategoryEquipmentViewModels)
        {
            var validator = new ReturnCategoryEquipmentViewModelValidator();
            var errorMessages = new List<string>();
            foreach (var viewModel in returnCategoryEquipmentViewModels)
            {
                var validationResult = validator.Validate(viewModel);
                if (!validationResult.IsValid)
                {
                    errorMessages.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
                }

                var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(viewModel.CategoryEquipmentId);
                if (categoryEquipment == null)
                {
                    errorMessages.Add($"Không tìm thấy thiết bị có category id là: {viewModel.CategoryEquipmentId}");
                }

                categoryEquipment.Quantity += viewModel.Quantity;
                _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);

                var logEquipBorrow = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByEquipmentIdAndUserAccountIdAndLogTypeAtClass(viewModel.CategoryEquipmentId, (Guid)viewModel.UserAccountId, LogType.AtClass);

                if(logEquipBorrow == null)
                {
                    throw new Exception("Giáo viên chưa mượn thiết bị nên không thể trả ");
                }

                if (logEquipBorrow.Status == StatusOfEquipment.Returned)
                {
                    throw new Exception("Giáo viên chưa mượn thiết bị nên không thể trả ");
                }

                if (logEquipBorrow.Quantity < viewModel.Quantity)
                {
                    throw new Exception($"Bạn đã trả quá số lượng mượn của thiết bị {categoryEquipment.Name}");
                }

                if (viewModel.Quantity < 1)
                {
                    throw new Exception("Bạn không được nhập số lượng nhỏ hơn 0");
                }

                var logEquipment = new LogEquipment
                {
                    EquipmentId = null,
                    UserAccountId = viewModel.UserAccountId,
                    Name = categoryEquipment.Name,
                    Code = categoryEquipment.Code,
                    Price = null,
                    Status = StatusOfEquipment.Returned,
                    RepairDate = null,
                    BorrowedDate = logEquipBorrow.BorrowedDate,
                    ReturnedDate = _currentTime.GetCurrentTime(),
                    ReturnedDealine = logEquipBorrow.BorrowedDate,
                    WarrantyPeriod = null,
                    PurchaseDate = null,
                    RoomId = null,
                    CategoryEquipmentId = viewModel.CategoryEquipmentId,
                    Quantity = viewModel.Quantity,
                    Note = viewModel.Note,
                    LogType = LogType.AtClass
                };
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
            }
            if (errorMessages.Any())
            {
                return false;
            }
            else
            {
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
        }

        public async Task<bool> ReturnForHomeCategoryEquipment(List<ReturnCategoryEquipmentViewModel> returnCategoryEquipmentViewModels)
        {
            var validator = new ReturnCategoryEquipmentViewModelValidator();
            var errorMessages = new List<string>();
            foreach (var viewModel in returnCategoryEquipmentViewModels)
            {
                var validationResult = validator.Validate(viewModel);
                if (!validationResult.IsValid)
                {
                    errorMessages.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
                }

                var categoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(viewModel.CategoryEquipmentId);
                if (categoryEquipment == null)
                {
                    errorMessages.Add($"Không tìm thấy thiết bị có category id là: {viewModel.CategoryEquipmentId}");
                }

                categoryEquipment.Quantity += viewModel.Quantity;
                _unitOfWork.CategoryEquipmentRepository.Update(categoryEquipment);

                var logEquipBorrow = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByEquipmentIdAndUserAccountIdAndLogTypeAtHome(viewModel.CategoryEquipmentId, (Guid)viewModel.UserAccountId, LogType.AtHome);

                if (logEquipBorrow == null)
                {
                    throw new Exception("Giáo viên chưa mượn thiết bị nên không thể trả ");
                }

                if (logEquipBorrow.Status == StatusOfEquipment.Returned)
                {
                    throw new Exception("Giáo viên chưa mượn thiết bị nên không thể trả ");
                }

                if (logEquipBorrow.Quantity < viewModel.Quantity)
                {
                    throw new Exception($"Bạn đã trả quá số lượng mượn của thiết bị {categoryEquipment.Name}");
                }

                if (viewModel.Quantity < 1)
                {
                    throw new Exception("Bạn không được nhập số lượng nhỏ hơn 0");
                }


                var logEquipment = new LogEquipment
                {
                    EquipmentId = null,
                    UserAccountId = viewModel.UserAccountId,
                    Name = categoryEquipment.Name,
                    Code = categoryEquipment.Code,
                    Price = null,
                    Status = StatusOfEquipment.Returned,
                    RepairDate = null,
                    BorrowedDate = logEquipBorrow.BorrowedDate,
                    ReturnedDate = _currentTime.GetCurrentTime(),
                    ReturnedDealine = logEquipBorrow.ReturnedDealine,
                    WarrantyPeriod = null,
                    PurchaseDate = null,
                    RoomId = null,
                    CategoryEquipmentId = viewModel.CategoryEquipmentId,
                    Quantity = viewModel.Quantity,
                    Note = viewModel.Note,
                    LogType = LogType.AtHome
                };
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
            }
            if (errorMessages.Any())
            {
                return false;
            }
            else
            {
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
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
            mapper.Quantity = 0;
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

        public async Task<List<EquipmentReportViewModel>> EquipmentReport()
        {

            var checkEquipment = await _unitOfWork.CategoryEquipmentRepository.GetAllAsync();

            var listReport = new List<EquipmentReportViewModel>();

            foreach (var item in checkEquipment)
            {
                listReport.Add(new EquipmentReportViewModel() { Id = item.Id, Name = item.Name, Quantity = item.Quantity, Code = item.Code });

            }

            return listReport;
        }

        public class EquipmentReportViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string? Code { get; set; }
        }

    }
}
