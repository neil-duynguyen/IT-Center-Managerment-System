using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Exams;
using KidProEdu.Application.Validations.SkillCertificates;
using KidProEdu.Application.Validations.Tags;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ExamService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<Exam> CreateExam(CreateExamViewModel2 createExamViewModel)
        {
            var validator = new CreateExamViewModelValidator();
            var validationResult = validator.Validate(createExamViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            /*var exitResult = await _unitOfWork.ExamRepository.GetExamByTestName(createExamViewModel.TestName);
            if (exitResult != null)
            {
                throw new Exception("Bài kiểm tra này đã tồn tại");
            }*/

            var mapper = _mapper.Map<Exam>(createExamViewModel);
            await _unitOfWork.ExamRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? mapper : throw new Exception("Tạo bài kiểm tra thất bại");
        }

        public async Task<Exam> CreateExamFinalPractice(CreateExamFinalPracticeViewModel createExamViewModel)
        {
            var validator = new CreateExamFinalPracticeViewModelValidator(_currentTime.GetCurrentTime());
            var validationResult = validator.Validate(createExamViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var exitResult = await _unitOfWork.ExamRepository.GetExamByTestName(createExamViewModel.TestName);
            if (exitResult != null)
            {
                throw new Exception("Bài kiểm tra này đã tồn tại");
            }

            var getNumberExamByClassId = _unitOfWork.ExamRepository.GetAllAsync().Result.Where(x => x.ClassId == createExamViewModel.ClassId).ToList();

            var mapper = _mapper.Map<Exam>(createExamViewModel);
            mapper.TestCode = "EFP" + getNumberExamByClassId.Count + 1;
            mapper.TestType = Domain.Enums.TestType.FinalPractice;
            await _unitOfWork.ExamRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? mapper : throw new Exception("Tạo bài kiểm tra thất bại");
        }

        public async Task<bool> DeleteExam(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy bài kiểm tra này");
            else
            {
                _unitOfWork.ExamRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa bài kiểm tra thất bại");
            }
        }

        public async Task<ExamViewModel> GetExamById(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<ExamViewModel>(result);
            return mapper;
        }

        public async Task<ExamViewModel> GetExamByTestName(string name)
        {
            var result = await _unitOfWork.ExamRepository.GetExamByTestName(name);
            var mapper = _mapper.Map<ExamViewModel>(result);
            return mapper;
        }

        public async Task<List<ExamViewModel>> GetExams()
        {
            var result = _unitOfWork.ExamRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<ExamViewModel>>(result);
            return mapper;
        }

        public async Task<List<ExamViewModel>> GetExamsByCourseId(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.GetExamByCourseId(id);
            var mapper = _mapper.Map<List<ExamViewModel>>(result);
            return mapper;
        }

        public async Task<List<ExamViewModel>> GetExamsByClassId(Guid id)
        {
            var result = await _unitOfWork.ExamRepository.GetAllAsync();
            var mapper = _mapper.Map<List<ExamViewModel>>(result.Where(x => x.ClassId == id));
            return mapper;
        }

        public async Task<bool> UpdateExam(UpdateExamViewModel updateExamViewModel)
        {
            var validator = new UpdateExamViewModelValidator();
            var validationResult = validator.Validate(updateExamViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var result = await _unitOfWork.ExamRepository.GetByIdAsync(updateExamViewModel.Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy bài kiểm tra");
            }

            /*var existingExam = await _unitOfWork.ExamRepository.GetExamByTestName(updateExamViewModel.TestName);
            if (existingExam != null)
            {
                if (existingExam.Id != updateExamViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }*/

            result.TestDuration = updateExamViewModel.TestDuration;
            result.TestDate = updateExamViewModel.TestDate;
            result.TestType = updateExamViewModel.TestType;
            result.CourseId = updateExamViewModel.CourseId;
            result.TestName = updateExamViewModel.TestName;
            _unitOfWork.ExamRepository.Update(result);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật bài kiểm tra thất bại");
        }
    }
}
