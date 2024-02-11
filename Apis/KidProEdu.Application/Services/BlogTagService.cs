using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Blogs;
using KidProEdu.Application.Validations.BlogTags;
using KidProEdu.Application.ViewModels.BlogTagViewModels;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class BlogTagService : IBlogTagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public BlogTagService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateBlogTag(CreateBlogTagViewModel createBlogTagViewModel)
        {
            var validator = new CreateBlogTagViewModelValidator();
            var validationResult = validator.Validate(createBlogTagViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var blogTags = _unitOfWork.BlogTagRepository.GetBlogTagByBlogId(createBlogTagViewModel.BlogId).Result.ToList();

            if (blogTags.Any())
            {
                if (blogTags.Any(tag => tag.TagId == createBlogTagViewModel.TagId))
                {
                    throw new Exception("TagId đã tồn tại");
                }
            }

            var mapper = _mapper.Map<BlogTag>(createBlogTagViewModel);
            await _unitOfWork.BlogTagRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo liên kết thất bại");
        }

        public async Task<bool> DeleteBlogTag(Guid id)
        {
            var result = await _unitOfWork.BlogTagRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy liên kết");
            else
            {
                _unitOfWork.BlogTagRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa liên kết thất bại");
            }
        }

        public async Task<BlogTag> GetBlogTagById(Guid id)
        {
            var result = await _unitOfWork.BlogTagRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<List<BlogTag>> GetBlogTags()
        {
            var results = _unitOfWork.BlogTagRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).ToList();
            return results;
        }

        public async Task<bool> UpdateBlogTag(UpdateBlogTagViewModel updateBlogTagViewModel)
        {
            var validator = new UpdateBlogTagViewModelValidator();
            var validationResult = validator.Validate(updateBlogTagViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var blogTags = _unitOfWork.BlogTagRepository.GetBlogTagByBlogId(updateBlogTagViewModel.BlogId).Result.ToList();

            if (blogTags.Any())
            {
                if (blogTags.Any(tag => tag.TagId == updateBlogTagViewModel.TagId))
                {
                    throw new Exception("TagId đã tồn tại");
                }
            }

            var mapper = _mapper.Map<BlogTag>(updateBlogTagViewModel);
            _unitOfWork.BlogTagRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật liên kết thất bại");
        }
    }
}
