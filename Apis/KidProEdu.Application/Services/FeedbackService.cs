using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Exams;
using KidProEdu.Application.Validations.Feedbacks;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.FeedBackViewModels;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateFeedback(CreateFeedBackViewModel createFeedBackViewModel)
        {
            var validator = new CreateFeedbackViewModelValidator();
            var validationResult = validator.Validate(createFeedBackViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var mapper = _mapper.Map<Feedback>(createFeedBackViewModel);
            await _unitOfWork.FeedbackRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo đánh giá thất bại");
        }

        public async Task<bool> DeleteFeedback(Guid id)
        {
            var result = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy đánh giá này");
            else
            {
                _unitOfWork.FeedbackRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa đánh giá thất bại");
            }
        }

        public async Task<List<FeedBackViewModel>> GetFeedbackByClassId(Guid classId)
        {
            var result = await _unitOfWork.FeedbackRepository.GetFeedbackByClassId(classId);
            var mapper = _mapper.Map<List<FeedBackViewModel>>(result);
            return mapper;
        }

        public async Task<FeedBackViewModel> GetFeedbackById(Guid id)
        {
            var result = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<FeedBackViewModel>(result);
            return mapper;
        }

        public async Task<List<FeedBackViewModel>> GetFeedbackByUserId(Guid userId)
        {
            var result = await _unitOfWork.FeedbackRepository.GetFeedbackByUserId(userId);
            var mapper = _mapper.Map<List<FeedBackViewModel>>(result);
            return mapper;
        }

        public async Task<List<FeedBackViewModel>> GetFeedbacks()
        {
            var result = _unitOfWork.FeedbackRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<FeedBackViewModel>>(result);
            return mapper;
        }

        public async Task<bool> UpdateFeedback(UpdateFeedBackViewModel updateFeedBackViewModel)
        {
            var validator = new UpdateFeedbackViewModelValidator();
            var validationResult = validator.Validate(updateFeedBackViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }
            var result = await _unitOfWork.FeedbackRepository.GetByIdAsync(updateFeedBackViewModel.Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy đánh giá");
            }


            result.UserId = updateFeedBackViewModel.UserId;
            result.ClassId = updateFeedBackViewModel.ClassId;
            result.RecipientId = updateFeedBackViewModel.RecipientId;
            result.Stars = updateFeedBackViewModel.Stars;
            result.Messages = updateFeedBackViewModel.Messages;
            _unitOfWork.FeedbackRepository.Update(result);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật đánh giá thất bại");
        }
    }
}
