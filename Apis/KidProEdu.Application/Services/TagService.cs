using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Tags;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KidProEdu.Application.Services
{

    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateTag(CreateTagViewModel createTagViewModel)
        {
            var validator = new CreateTagViewModelValidator();
            var validationResult = validator.Validate(createTagViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var tag = await _unitOfWork.TagRepository.GetTagByTagName(createTagViewModel.TagName);
            if(!tag.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<Tag>(createTagViewModel);
            await _unitOfWork.TagRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo Tag thất bại");

        }

        public async Task<bool> DeleteTag(Guid tagId)
        {
            var result = await _unitOfWork.TagRepository.GetByIdAsync(tagId);

            if (result == null)
                throw new Exception("Không tìm thấy Tag này");
            else
            {
                _unitOfWork.TagRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xoá Tag thất bại");
            }
        }

        public async Task<Tag> GetTagById(Guid tagId)
        {
            var tag = await _unitOfWork.TagRepository.GetByIdAsync(tagId);
            return tag;
        }

        public async Task<List<Tag>> GetTags()
        {
            var tags = await _unitOfWork.TagRepository.GetAllAsync();
            return tags;
        }

        public async Task<bool> UpdateTag(UpdateTagViewModel updateTagViewModel)
        {
            var validator = new UpdateTagViewModelValidator();
            var validationResult = validator.Validate(updateTagViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var tag = await _unitOfWork.TagRepository.GetTagByTagName(updateTagViewModel.TagName);
            if (!tag.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<Tag>(updateTagViewModel);
            _unitOfWork.TagRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật Tag thất bại");
        }
    }
}
