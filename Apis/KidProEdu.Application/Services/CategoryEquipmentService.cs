using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.CategoryEquipments;
using KidProEdu.Application.Validations.Locations;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Domain.Entities;
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
