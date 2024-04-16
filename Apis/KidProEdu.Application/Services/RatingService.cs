using AutoMapper;
using KidProEdu.Application.Hubs;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Blogs;
using KidProEdu.Application.Validations.Locations;
using KidProEdu.Application.Validations.Ratings;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateRating(CreateRatingViewModel createRatingViewModel)
        {
            var validator = new CreateRatingViewModelValidator();
            var validationResult = validator.Validate(createRatingViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var mapper = _mapper.Map<Rating>(createRatingViewModel);
            mapper.Date = DateTime.Now;
            await _unitOfWork.RatingRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo đánh giá thất bại");
        }

        public async Task<bool> DeleteRating(Guid id)
        {
            var result = await _unitOfWork.RatingRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy đánh giá này");
            else
            {
                _unitOfWork.RatingRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa đánh giá thất bại");
            }
        }

        public async Task<RatingViewModel> GetRatingById(Guid id)
        {
            var results = await _unitOfWork.RatingRepository.GetByIdAsync(id);

            var mapper = _mapper.Map<RatingViewModel>(results);

            return mapper;
        }

        public async Task<List<RatingViewModel>> GetRatings()
        {
            var results = _unitOfWork.RatingRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var mapper = _mapper.Map<List<RatingViewModel>>(results);

            return mapper;
        }

        public async Task<List<RatingViewModel>> GetRatingsByCourseId(Guid courseId)
        {
            var results = await _unitOfWork.RatingRepository.GetListRatingByCourseId(courseId);

            var mapper = _mapper.Map<List<RatingViewModel>>(results);

            return mapper;
        }

        public async Task<bool> UpdateRating(UpdateRatingViewModel updateRatingViewModel)
        {
            var validator = new UpdateRatingViewModelValidator();
            var validationResult = validator.Validate(updateRatingViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(updateRatingViewModel.Id);
            if (rating == null)
            {
                throw new Exception("Không tìm thấy đánh giá");
            }


            rating.Date = DateTime.Now;
            rating.Comment = updateRatingViewModel.Comment;
            rating.StarNumber = updateRatingViewModel.StarNumber;
            _unitOfWork.RatingRepository.Update(rating);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật đánh giá thất bại");
        }
    }
}
