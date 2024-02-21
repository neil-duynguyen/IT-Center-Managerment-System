using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Blogs;
using KidProEdu.Application.Validations.CategoryEquipments;
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
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public BlogService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateBlog(CreateBlogViewModel createBlogViewModel)
        {
            var validator = new CreateBlogViewModelValidator();
            var validationResult = validator.Validate(createBlogViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var blogObj = _mapper.Map<Blog>(createBlogViewModel);

            IList<Tag> tags = new List<Tag>();

            if (createBlogViewModel.TagIds.Count != 0)
            {
                foreach (var tag in createBlogViewModel.TagIds)
                {
                    tags.Add(await _unitOfWork.TagRepository.GetByIdAsync(tag));
                }
                //gán listTag vào Blog
                blogObj.Tags = tags;
            }


            await _unitOfWork.BlogRepository.AddAsync(blogObj);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo bài viết thất bại");
        }

        public async Task<bool> DeleteBlog(Guid id)
        {
            var result = await _unitOfWork.BlogRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy bài viết");
            else
            {
                _unitOfWork.BlogRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa bài viết thất bại");
            }
        }

        public async Task<BlogViewModel> GetBlogById(Guid id)
        {
            var result = await _unitOfWork.BlogRepository.GetByIdAsync(id);

            return _mapper.Map<BlogViewModel>(result);
        }

        public async Task<List<BlogViewModel>> GetBlogs()
        {
            var results = _unitOfWork.BlogRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate);

            var mapper = _mapper.Map<List<BlogViewModel>>(results);

            return mapper;
        }

        public async Task<List<BlogViewModel>> GetBlogWithUserByBlogId(Guid id)
        {
            var result = _unitOfWork.BlogRepository.GetAllAsync().Result.Where(x => x.UserId == id).ToList();

            return _mapper.Map<List<BlogViewModel>>(result);
        }

        public async Task<bool> UpdateBlog(UpdateBlogViewModel updateBlogViewModel)
        {
            var validator = new UpdateBlogViewModelValidator();
            var validationResult = validator.Validate(updateBlogViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var mapper = _mapper.Map<Blog>(updateBlogViewModel);
            _unitOfWork.BlogRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật bài viết thất bại");
        }
    }
}
