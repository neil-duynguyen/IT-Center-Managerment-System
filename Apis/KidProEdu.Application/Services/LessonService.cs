﻿using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
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

            var checkDuration = await _unitOfWork.CourseRepository.GetByIdAsync(createLessonViewModel.CourseId);
            if (checkDuration == null) throw new InvalidOperationException("Không tìm thấy môn học.");

            var sumDurationLesson = checkDuration.Lessons.Sum(x => x.Duration);
            if (sumDurationLesson + createLessonViewModel.Duration > checkDuration.DurationTotal)
                throw new Exception($"Thời gian (slot) phải <= {checkDuration.DurationTotal - sumDurationLesson}");

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(createLessonViewModel.CourseId);
            if (course.Lessons.FirstOrDefault(x => x.Name.ToLower() == createLessonViewModel.Name.ToLower() && !x.IsDeleted) != null)
                throw new Exception("Tên bài học đã tồn tại.");

            var mapper = _mapper.Map<Lesson>(createLessonViewModel);

            IList<Equipment> equipments = new List<Equipment>();

            if (createLessonViewModel.EquipmentId.Count != 0)
            {
                foreach (var equipment in createLessonViewModel.EquipmentId)
                {
                    equipments.Add(await _unitOfWork.EquipmentRepository.GetByIdAsync(equipment));
                }

                mapper.Equipments = equipments;
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

            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(updateLessonViewModel.Id);
            if (lesson == null)
            {
                throw new Exception("Không tìm thấy bài học");
            }

            var existingLesson = await _unitOfWork.LessonRepository.GetLessonByName(updateLessonViewModel.Name);
            if (existingLesson != null)
            {
                if (existingLesson.Id != updateLessonViewModel.Id)
                {
                    throw new Exception("Bài học đã tồn tại");
                }
            }

            var mapper = _mapper.Map<Lesson>(lesson);
            mapper.LessonNumber = updateLessonViewModel.LessonNumber;
            mapper.Duration = updateLessonViewModel.Duration;
            mapper.CourseId = updateLessonViewModel.CourseId;
            mapper.Name = updateLessonViewModel.Name;
            mapper.Description = updateLessonViewModel.Description;
            _unitOfWork.LessonRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật bài học thất bại");
        }
    }
}
