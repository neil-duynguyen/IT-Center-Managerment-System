using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Attendances;
using KidProEdu.Application.Validations.ChildrenAnswers;
using KidProEdu.Application.Validations.Exams;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.ChildrenAnswerViewModels;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class ChildrenAnswerService : IChildrenAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ChildrenAnswerService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateChildrenAnswers(List<CreateChildrenAnswerViewModel> createChildrenAnswerViewModel)
        {
            var validator = new CreateChildrenAnswerViewModelValidator();
            foreach (var childrenAnswerViewModel in createChildrenAnswerViewModel)
            {
                var validationResult = validator.Validate(childrenAnswerViewModel);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }
            }
            var mapper = _mapper.Map<List<ChildrenAnswer>>(createChildrenAnswerViewModel);
            await _unitOfWork.ChildrenAnswerRepository.AddRangeAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo câu trả lời thất bại");
        }

        /*public async Task<bool> CreateChildrenAnswer(CreateChildrenAnswerViewModel createChildrenAnswerViewModel)
        {
            var validator = new CreateChildrenAnswerViewModelValidator();
            var validationResult = validator.Validate(createChildrenAnswerViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var exitResult = await _unitOfWork.ChildrenAnswerRepository.GetChildrenAnswerWithChildrenProfileIdExamIdQuestionId(createChildrenAnswerViewModel.ChildrenProfileId, createChildrenAnswerViewModel.ExamId, createChildrenAnswerViewModel.QuestionId);
            if (exitResult != null)
            {
                throw new Exception("Câu trả lời này đã tồn tại");
            }

            var mapper = _mapper.Map<ChildrenAnswer>(createChildrenAnswerViewModel);
            await _unitOfWork.ChildrenAnswerRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo câu trả lời tra thất bại");
        }*/

        public async Task<bool> DeleteChildrenAnswer(Guid id)
        {
            var result = await _unitOfWork.ChildrenAnswerRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy câu trả lời này");
            else
            {
                _unitOfWork.ChildrenAnswerRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa câu trả lời thất bại");
            }
        }

        public async Task<ChildrenAnswerViewModel> GetChildrenAnswerById(Guid id)
        {
            var result = await _unitOfWork.ChildrenAnswerRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<ChildrenAnswerViewModel>(result);
            return mapper;
        }

        public async Task<List<QuestionViewModel>> GetChildrenAnswers(Guid childrenId, Guid examId)
        {
            var result = _unitOfWork.ChildrenAnswerRepository.GetAllAsync().Result.Where(x => x.ChildrenProfileId == childrenId && x.ExamId == examId).OrderByDescending(x => x.CreationDate).ToList();

            List<QuestionViewModel> questionViewModel = new List<QuestionViewModel>();

            foreach (var item in result)
            {
                var mapper = _mapper.Map<QuestionViewModel>(await _unitOfWork.QuestionRepository.GetByIdAsync((Guid)item.QuestionId));
                mapper.Answer = item.Answer;
                mapper.ScorePerQuestion = item.ScorePerQuestion;
                questionViewModel.Add(mapper);
            }
            return questionViewModel;
        }

        public async Task<bool> UpdateChildrenAnswer(UpdateChildrenAnswerViewModel updateChildrenAnswerView)
        {
            var validator = new UpdateChildrenAnswerViewModelValidator();
            var validationResult = validator.Validate(updateChildrenAnswerView);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var result = await _unitOfWork.ChildrenAnswerRepository.GetByIdAsync(updateChildrenAnswerView.Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy câu trả lời");
            }

            var existing = await _unitOfWork.ChildrenAnswerRepository.GetChildrenAnswerWithChildrenProfileIdExamIdQuestionId(updateChildrenAnswerView.ChildrenProfileId, updateChildrenAnswerView.ExamId, updateChildrenAnswerView.ExamId);
            if (existing != null)
            {
                if (existing.Id != updateChildrenAnswerView.Id)
                {
                    throw new Exception("Câu trả lời đã tồn tại");
                }
            }

            result.ChildrenProfileId = updateChildrenAnswerView.ChildrenProfileId;
            result.ExamId = updateChildrenAnswerView.ExamId;
            result.QuestionId = updateChildrenAnswerView.QuestionId;
            result.Answer = updateChildrenAnswerView.Answer;
            result.ScorePerQuestion = updateChildrenAnswerView.ScorePerQuestion;
            _unitOfWork.ChildrenAnswerRepository.Update(result);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật câu trả lời thất bại");
        }
    }
}
