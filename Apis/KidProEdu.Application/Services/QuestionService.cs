using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2013.Excel;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Questions;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System;

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
                    //continue;
                    throw new Exception("Câu hỏi này đã tồn tại trong 1 bài học");
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

        //createTestEntry
        public async Task<List<Question>> CreateTestEntry(CreateExamEntryViewModel createExamEntryViewModel)
        {
            if (createExamEntryViewModel == null)
            {
                return null;
            }

            var children = await _unitOfWork.ChildrenRepository.GetByIdAsync(createExamEntryViewModel.ChildrenId);

            if (children == null)
            {
                return null;
            }
           
            var totalQuestion = createExamEntryViewModel.TotalQuestion;
            if(totalQuestion == null || totalQuestion <= 0)
            {
                return null;
            }

            var oldOfChildren = _currentTime.GetCurrentTime().Year - children.BirthDay.Year;

            Domain.Enums.QuestionType typeQuestion;
            if(oldOfChildren < 8)
            {
                typeQuestion = Domain.Enums.QuestionType.Entry8And9;
            }
            else if(oldOfChildren == 8 || oldOfChildren == 9) {
                typeQuestion = Domain.Enums.QuestionType.Entry8And9;
            }
            else if (oldOfChildren == 10 || oldOfChildren == 11)
            {
                typeQuestion = Domain.Enums.QuestionType.Entry10And11;
            }
            else if (oldOfChildren == 12 || oldOfChildren == 13)
            {
                typeQuestion = Domain.Enums.QuestionType.Entry12And13;
            }
            else if (oldOfChildren == 14 || oldOfChildren == 15)
            {
                typeQuestion = Domain.Enums.QuestionType.Entry14And15;
            }
            else
            {
                typeQuestion = Domain.Enums.QuestionType.Entry14And15;
            }

            Random random = new Random();

            var questions = await _unitOfWork.QuestionRepository.GetQuestionByType(typeQuestion);

            // phân chia danh sách câu hỏi thành các danh sách con theo cấp độ
            var level1Questions = questions.Where(q => q.Level == 1).ToList();
            var level2Questions = questions.Where(q => q.Level == 2).ToList();
            var level3Questions = questions.Where(q => q.Level == 3).ToList();

            // tính số lượng câu hỏi mong muốn cho mỗi cấp độ dựa trên tỉ lệ
            int totalQuestions = (int)createExamEntryViewModel.TotalQuestion;
            int level1Count = (int)(totalQuestions * 0.4);
            int level2Count = (int)(totalQuestions * 0.3);
            int level3Count = totalQuestions - level1Count - level2Count; // còn lại cho cấp độ 3

            // lấy ngẫu nhiên các câu hỏi từ mỗi danh sách con
            var randomLevel1Questions = level1Questions.OrderBy(x => random.Next()).Take(level1Count);
            var randomLevel2Questions = level2Questions.OrderBy(x => random.Next()).Take(level2Count);
            var randomLevel3Questions = level3Questions.OrderBy(x => random.Next()).Take(level3Count);

            // kết hợp danh sách câu hỏi từ mỗi cấp độ vào danh sách câu hỏi cuối cùng
            var randomList = randomLevel1Questions.Concat(randomLevel2Questions).Concat(randomLevel3Questions).ToList();
            return randomList;
        }

        public async Task<Question> GetQuestionById(Guid questionId)
        {
            var question = await _unitOfWork.QuestionRepository.GetByIdAsync(questionId);
            return question;
        }

        public async Task<List<QuestionByLessonViewModel>> CreateTest(List<CreateExamViewModel> createExamViewModels)
        {

            /*
              CreateExamViewModel
                        - lấy test đầu vào (truyền type Entry bắt buộc):
                                => lấy list question đầu vào chỉ cần truyền mảng 1 phần tử type Entry là bắt buộc còn lại null
                                => lấy list 50 questions đầu vào lessonId = null, total = 50, type = Entry
                        - lấy test theo lesson (truyền type Course bắt buộc):
                                => lấy all list question theo lesson => truyền mảng 1 phần tử lessonId với totalQuestion = null
                                => lấy list 50 questions theo lesson => truyền mảng 1 phần tử lessonId kèm totalQuestion = 50
                                => lấy list 50 questions theo list lesson => truyền list (lessonId kèm totalQuestion = 50)
            */

            //var questions = new List<Question> { };
            var listModel = new List<QuestionByLessonViewModel>();
            if (createExamViewModels.IsNullOrEmpty())
            {
                throw new Exception("Dữ liệu không hợp lệ");
            }

            if (createExamViewModels.Count == 1)
            {
                if (createExamViewModels.FirstOrDefault().Type.Equals(Domain.Enums.QuestionType.Entry10And11)
                    && createExamViewModels.FirstOrDefault().TotalQuestion == null)
                {
                    var vm = new QuestionByLessonViewModel
                    {
                        Questions = await _unitOfWork.QuestionRepository.GetQuestionByType(createExamViewModels[0].Type),
                        Type = "Entry"
                    };
                    listModel.Add(vm);
                }
                else if (createExamViewModels.FirstOrDefault().Type.Equals(Domain.Enums.QuestionType.Entry10And11)
                    && createExamViewModels.FirstOrDefault().TotalQuestion != null)
                {
                    Random random = new Random();

                    var questions = await _unitOfWork.QuestionRepository.GetQuestionByType(Domain.Enums.QuestionType.Entry10And11);

                    // phân chia danh sách câu hỏi thành các danh sách con theo cấp độ
                    var level1Questions = questions.Where(q => q.Level == 1).ToList();
                    var level2Questions = questions.Where(q => q.Level == 2).ToList();
                    var level3Questions = questions.Where(q => q.Level == 3).ToList();

                    // tính số lượng câu hỏi mong muốn cho mỗi cấp độ dựa trên tỉ lệ
                    int totalQuestions = (int)createExamViewModels[0].TotalQuestion;
                    int level1Count = (int)(totalQuestions * 0.4);
                    int level2Count = (int)(totalQuestions * 0.4);
                    int level3Count = totalQuestions - level1Count - level2Count; // còn lại cho cấp độ 3

                    // lấy ngẫu nhiên các câu hỏi từ mỗi danh sách con
                    var randomLevel1Questions = level1Questions.OrderBy(x => random.Next()).Take(level1Count);
                    var randomLevel2Questions = level2Questions.OrderBy(x => random.Next()).Take(level2Count);
                    var randomLevel3Questions = level3Questions.OrderBy(x => random.Next()).Take(level3Count);

                    // kết hợp danh sách câu hỏi từ mỗi cấp độ vào danh sách câu hỏi cuối cùng
                    var randomList = randomLevel1Questions.Concat(randomLevel2Questions).Concat(randomLevel3Questions).ToList();

                    var vm = new QuestionByLessonViewModel
                    {
                        Questions = randomList,
                        Type = "Entry"
                    };
                    listModel.Add(vm);

                }
                else if (createExamViewModels.FirstOrDefault().Type.Equals(Domain.Enums.QuestionType.Course)
                    && createExamViewModels.FirstOrDefault().TotalQuestion == null)
                {
                    var vm = new QuestionByLessonViewModel
                    {
                        LessonId = createExamViewModels.FirstOrDefault().LessonId,
                        Questions = await _unitOfWork.QuestionRepository.GetQuestionByLesson((Guid)createExamViewModels.FirstOrDefault().LessonId),
                        Type = "Course"
                    };
                    listModel.Add(vm);
                }
                else if (createExamViewModels.FirstOrDefault().Type.Equals(Domain.Enums.QuestionType.Course)
                    && createExamViewModels.FirstOrDefault().TotalQuestion != null)
                {
                    Random random = new Random();

                    var questions = _unitOfWork.QuestionRepository.GetQuestionByType(Domain.Enums.QuestionType.Course)
                        .Result.Where(x => x.LessionId == createExamViewModels.FirstOrDefault().LessonId);

                    // phân chia danh sách câu hỏi thành các danh sách con theo cấp độ
                    var level1Questions = questions.Where(q => q.Level == 1).ToList();
                    var level2Questions = questions.Where(q => q.Level == 2).ToList();
                    var level3Questions = questions.Where(q => q.Level == 3).ToList();

                    // tính số lượng câu hỏi mong muốn cho mỗi cấp độ dựa trên tỉ lệ
                    int totalQuestions = (int)createExamViewModels[0].TotalQuestion;
                    int level1Count = (int)(totalQuestions * 0.4);
                    int level2Count = (int)(totalQuestions * 0.4);
                    int level3Count = totalQuestions - level1Count - level2Count; // còn lại cho cấp độ 3

                    // lấy ngẫu nhiên các câu hỏi từ mỗi danh sách con
                    var randomLevel1Questions = level1Questions.OrderBy(x => random.Next()).Take(level1Count);
                    var randomLevel2Questions = level2Questions.OrderBy(x => random.Next()).Take(level2Count);
                    var randomLevel3Questions = level3Questions.OrderBy(x => random.Next()).Take(level3Count);

                    // kết hợp danh sách câu hỏi từ mỗi cấp độ vào danh sách câu hỏi cuối cùng
                    var randomList = randomLevel1Questions.Concat(randomLevel2Questions).Concat(randomLevel3Questions).ToList();

                    var vm = new QuestionByLessonViewModel
                    {
                        LessonId = createExamViewModels.FirstOrDefault().LessonId,
                        Questions = randomList,
                        Type = "Entry"
                    };
                    listModel.Add(vm);
                }
            }
            else
            {
                Random random = new Random();

                foreach (var item in createExamViewModels)
                {
                    var questions = _unitOfWork.QuestionRepository.GetQuestionByLesson((Guid)item.LessonId)
                        .Result.Where(x => x.LessionId == item.LessonId);

                    // phân chia danh sách câu hỏi thành các danh sách con theo cấp độ
                    var level1Questions = questions.Where(q => q.Level == 1).ToList();
                    var level2Questions = questions.Where(q => q.Level == 2).ToList();
                    var level3Questions = questions.Where(q => q.Level == 3).ToList();

                    // tính số lượng câu hỏi mong muốn cho mỗi cấp độ dựa trên tỉ lệ
                    int totalQuestions = (int)item.TotalQuestion;
                    int level1Count = (int)(totalQuestions * 0.4);
                    int level2Count = (int)(totalQuestions * 0.4);
                    int level3Count = totalQuestions - level1Count - level2Count; // còn lại cho cấp độ 3

                    // lấy ngẫu nhiên các câu hỏi từ mỗi danh sách con
                    var randomLevel1Questions = level1Questions.OrderBy(x => random.Next()).Take(level1Count);
                    var randomLevel2Questions = level2Questions.OrderBy(x => random.Next()).Take(level2Count);
                    var randomLevel3Questions = level3Questions.OrderBy(x => random.Next()).Take(level3Count);

                    // kết hợp danh sách câu hỏi từ mỗi cấp độ vào danh sách câu hỏi cuối cùng
                    var randomList = randomLevel1Questions.Concat(randomLevel2Questions).Concat(randomLevel3Questions).ToList();

                    var vm = new QuestionByLessonViewModel
                    {
                        LessonId = item.LessonId,
                        Questions = randomList,
                        Type = "Course"
                    };
                    listModel.Add(vm);
                }
            }
            return listModel;
        }

        public async Task<List<Question2ViewModel>> GetQuestions()
        {
            var questions = _unitOfWork.QuestionRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<Question2ViewModel>>(questions);
            return mapper;
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

        public async Task<List<Question2ViewModel>> GetQuestionsByType(Domain.Enums.QuestionType type)
        {
            var questions = _unitOfWork.QuestionRepository.GetQuestionByType(type).Result.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<Question2ViewModel>>(questions);
            return mapper;
        }
    }
}
