using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Hangfire.MemoryStorage.Utilities;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Lessons;
using KidProEdu.Application.Validations.Ratings;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public LessonService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateLesson(CreateLessonViewModel createLessonViewModel)
        {
            var validator = new CreateLessonViewModelValidator();
            var validationResult = validator.Validate(createLessonViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            if (createLessonViewModel.TypeOfPractice == Domain.Enums.TypeOfPractice.Group && createLessonViewModel.GroupSize < 2) throw new Exception("Nhóm không thể ít hơn 2 người");

            var checkDuration = await _unitOfWork.CourseRepository.GetByIdAsync(createLessonViewModel.CourseId);
            var filterLesson = checkDuration.Lessons.Where(x => !x.IsDeleted).ToList();
            if (filterLesson == null) throw new InvalidOperationException("Không tìm thấy bài học.");

            var sumDurationLesson = filterLesson.Sum(x => x.Duration);
            if (sumDurationLesson + createLessonViewModel.Duration > checkDuration.DurationTotal)
            {
                var remainingDuration = checkDuration.DurationTotal - sumDurationLesson;
                var errorMessage = remainingDuration > 0 ? $"Thời gian (slot) phải nhỏ hơn hoặc bằng {remainingDuration}" : "Không thể thêm bài học vì thời lượng tổng khoá học đã hết.";
                throw new Exception(errorMessage);
            }

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(createLessonViewModel.CourseId);
            if (course.Lessons.FirstOrDefault(x => x.Name.ToLower() == createLessonViewModel.Name.ToLower() && !x.IsDeleted) != null)
                throw new Exception("Tên bài học đã tồn tại.");

            var mapper = _mapper.Map<Lesson>(createLessonViewModel);
            mapper.LessonNumber = course.Lessons.Where(x => !x.IsDeleted).Count() + 1;

            IList<CategoryEquipment> equipments = new List<CategoryEquipment>();

            if (createLessonViewModel.EquipmentId.Count != 0)
            {
                foreach (var equipment in createLessonViewModel.EquipmentId)
                {
                    equipments.Add(await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(equipment));
                }

                mapper.CategoryEquipments = equipments;
            }

            await _unitOfWork.LessonRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo bài học thất bại");
        }

        public async Task<bool> DeleteLesson(Guid id)
        {
            var result = await _unitOfWork.LessonRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy bài học này");
            else
            {
                _unitOfWork.LessonRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa bài học thất bại");
            }
        }

        public async Task<LessonViewModel> GetLessonById(Guid id)
        {
            var results = await _unitOfWork.LessonRepository.GetByIdAsync(id);

            var mapper = _mapper.Map<LessonViewModel>(results);

            return mapper;
        }

        public async Task<List<LessonViewModel>> GetLessons()
        {
            var results = _unitOfWork.LessonRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var mapper = _mapper.Map<List<LessonViewModel>>(results);

            return mapper;
        }

        public async Task<List<LessonViewModel>> GetLessonsByCourseId(Guid courseId)
        {
            var results = await _unitOfWork.LessonRepository.GetLessonsByCourseId(courseId);

            var mapper = _mapper.Map<List<LessonViewModel>>(results);

            return mapper;
        }

        public async Task<bool> UpdateLesson(UpdateLessonViewModel updateLessonViewModel)
        {
            var validator = new UpdateLessonViewModelValidator();
            var validationResult = validator.Validate(updateLessonViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            if (updateLessonViewModel.TypeOfPractice == Domain.Enums.TypeOfPractice.Group && updateLessonViewModel.GroupSize < 2) throw new Exception("Nhóm không thể ít hơn 2 người");

            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(updateLessonViewModel.Id);
            if (lesson == null)
            {
                throw new Exception("Không tìm thấy bài học");
            }

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(updateLessonViewModel.CourseId);
            if (course.Lessons.FirstOrDefault(x => x.Name.ToLower() == updateLessonViewModel.Name.ToLower() && x.Id != updateLessonViewModel.Id && !x.IsDeleted) != null)
                throw new Exception("Tên bài học đã tồn tại.");

            var mapper = _mapper.Map(updateLessonViewModel, lesson);
            mapper.Duration = updateLessonViewModel.Duration;
            mapper.CourseId = updateLessonViewModel.CourseId;
            mapper.Name = updateLessonViewModel.Name;
            mapper.Description = updateLessonViewModel.Description;

            IList<CategoryEquipment> equipments = new List<CategoryEquipment>();

            if (updateLessonViewModel.EquipmentId.Count != 0)
            {
                foreach (var equipment in updateLessonViewModel.EquipmentId)
                {
                    equipments.Add(await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(equipment));
                }

                mapper.CategoryEquipments = equipments;
            }

            _unitOfWork.LessonRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật bài học thất bại");
        }
    }
}
