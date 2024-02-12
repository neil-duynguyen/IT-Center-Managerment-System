using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.CategoryEquipments;
using KidProEdu.Application.Validations.TrainingProgramCategories;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.TrainingProgramCategoryViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class TrainingProgramCategoryService : ITrainingProgramCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public TrainingProgramCategoryService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateTrainingProgramCategory(CreateTrainingProgramCategoryViewModel createTrainingProgramCategoryViewModel)
        {
            var validator = new CreateTrainingProgramCategoryViewModelValidator();
            var validationResult = validator.Validate(createTrainingProgramCategoryViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var trainingProgramCategory = await _unitOfWork.TrainingProgramCategoryRepository.GetTrainingProgramCategoryByName(createTrainingProgramCategoryViewModel.Name);
            if (!trainingProgramCategory.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<TrainingProgramCategory>(createTrainingProgramCategoryViewModel);
            await _unitOfWork.TrainingProgramCategoryRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo chương trình đào tạo thất bại");
        }

        public async Task<bool> DeleteTrainingProgramCategory(Guid id)
        {
            var result = await _unitOfWork.TrainingProgramCategoryRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy chương trình đào tạo");
            else
            {
                _unitOfWork.TrainingProgramCategoryRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa chương trình đào tạo thất bại");
            }
        }

        public async Task<List<TrainingProgramCategory>> GetTrainingProgramCategories()
        {
            var results = _unitOfWork.TrainingProgramCategoryRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).ToList();
            return results;
        }

        public async Task<TrainingProgramCategory> GetTrainingProgramCategoryById(Guid id)
        {
            var result = await _unitOfWork.TrainingProgramCategoryRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<bool> UpdateTrainingProgramCategory(UpdateTrainingProgramCategoryViewModel updateTrainingProgramCategoryViewModel)
        {
            var validator = new UpdateTrainingProgramCategoryViewModelValidator();
            var validationResult = validator.Validate(updateTrainingProgramCategoryViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var trainingProgramCategory = await _unitOfWork.TrainingProgramCategoryRepository.GetTrainingProgramCategoryByName(updateTrainingProgramCategoryViewModel.Name);
            if (!trainingProgramCategory.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<TrainingProgramCategory>(updateTrainingProgramCategoryViewModel);
            _unitOfWork.TrainingProgramCategoryRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật chương trình đào tạo thất bại");
        }
    }
}
