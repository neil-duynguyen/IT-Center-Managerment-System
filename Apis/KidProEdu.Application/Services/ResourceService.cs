using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Divisions;
using KidProEdu.Application.Validations.Lessons;
using KidProEdu.Application.Validations.Resources;
using KidProEdu.Application.Validations.Skills;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.ResourceViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ResourceService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateResource(CreateResourceViewModel createResourceViewModel)
        {
            var validator = new CreateResourceViewModelValidator();
            var validationResult = validator.Validate(createResourceViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var mapper = _mapper.Map<Resource>(createResourceViewModel);
            await _unitOfWork.ResourceRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo nguồn tài liệu thất bại");
        }

        public async Task<bool> DeleteResource(Guid id)
        {
            var result = await _unitOfWork.ResourceRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy nguồn tài liệu này");
            else
            {
                _unitOfWork.ResourceRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa nguồn tài liệu thất bại");
            }
        }

        public async Task<ResourceViewModel> GetResourceById(Guid id)
        {
            var result = await _unitOfWork.ResourceRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<ResourceViewModel>(result);
            return mapper;
        }

        public async Task<List<ResourceViewModel>> GetResources()
        {
            var result = _unitOfWork.ResourceRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<ResourceViewModel>>(result);
            return mapper;
        }

        public async Task<List<ResourceViewModel>> GetResourcesByCourseId(Guid id)
        {
            var result = await _unitOfWork.ResourceRepository.GetResourcesByCourseId(id);
            var mapper = _mapper.Map<List<ResourceViewModel>>(result);
            return mapper;
        }

        public async Task<bool> UpdateResource(UpdateResourceViewModel updateResourceViewModel)
        {
            var validator = new UpdateResourceViewModelValidator();
            var validationResult = validator.Validate(updateResourceViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var result = await _unitOfWork.ResourceRepository.GetByIdAsync(updateResourceViewModel.Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy nguồn tài liệu");
            }
            var mapper = _mapper.Map<Resource>(result);
            mapper.CourseId = updateResourceViewModel.CourseId;
            mapper.Url = updateResourceViewModel.Url;
            mapper.Description = updateResourceViewModel.Description;
            _unitOfWork.ResourceRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật nguồn tài liệu thất bại");
        }
    }
}
