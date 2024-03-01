using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Questions;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace KidProEdu.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateQuestion(CreateQuestionViewModel[] createQuestionViewModel)
        {
            var validator = new CreateQuestionViewModelValidator();
            foreach (var item in createQuestionViewModel)
            {
                var validationResult = validator.Validate(item);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }

                var question = await _unitOfWork.QuestionRepository.GetQuestionByTitle(item.Title);
                if (!question.IsNullOrEmpty())
                {
                    continue;
                }

                var mapper = _mapper.Map<Question>(item);
                await _unitOfWork.QuestionRepository.AddAsync(mapper);
            }
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo câu hỏi thất bại");

        }

        public async Task<bool> DeleteQuestion(Guid QuestionId)
        {
            var result = await _unitOfWork.QuestionRepository.GetByIdAsync(QuestionId);

            if (result == null)
                throw new Exception("Không tìm thấy câu hỏi này");
            else
            {
                _unitOfWork.QuestionRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa câu hỏi thất bại");
            }
        }

        public async Task<Question> GetQuestionById(Guid questionId)
        {
            var question = await _unitOfWork.QuestionRepository.GetByIdAsync(questionId);
            return question;
        }
        
        public async Task<List<Question>> GetQuestionByLesson(Guid lessonId)
        {
            var question = await _unitOfWork.QuestionRepository.GetQuestionByLesson(lessonId);
            return question;
        }

        public async Task<List<Question>> GetQuestions()
        {
            var questions = _unitOfWork.QuestionRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreationDate).ToList();
            return questions;
        }

        public async Task<bool> UpdateQuestion(UpdateQuestionViewModel updateQuestionViewModel)
        {
            var validator = new UpdateQuestionViewModelValidator();
            var validationResult = validator.Validate(updateQuestionViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var question = await _unitOfWork.QuestionRepository.GetByIdAsync(updateQuestionViewModel.Id);
            if (question == null)
            {
                throw new Exception("Không tìm thấy câu hỏi");
            }

            var existingQuestion = await _unitOfWork.QuestionRepository.GetQuestionByTitle(updateQuestionViewModel.Title);
            if (!existingQuestion.IsNullOrEmpty())
            {
                if (existingQuestion.FirstOrDefault().Id != updateQuestionViewModel.Id)
                {
                    throw new Exception("Câu hỏi đã tồn tại");
                }
            }

            var mapper = _mapper.Map(updateQuestionViewModel, question);

            _unitOfWork.QuestionRepository.Update(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật câu hỏi thất bại");
        }
    }
}
