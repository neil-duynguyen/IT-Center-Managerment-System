using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Semesters;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public SemesterService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateSemester(CreateSemesterViewModel createSemesterViewModel)
        {
            var validator = new CreateSemesterViewModelValidator();
            var validationResult = validator.Validate(createSemesterViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var semester = await _unitOfWork.SemesterRepository.GetSemesterBySemesterName(createSemesterViewModel.SemesterName);
            if (!semester.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            semester = await _unitOfWork.SemesterRepository.GetSemesterByStartDate(createSemesterViewModel.StartDate);
            if (!semester.IsNullOrEmpty())
            {
                throw new Exception("Ngày bắt đầu đã tồn tại");
            }

            var mapper = _mapper.Map<Semester>(createSemesterViewModel);
            await _unitOfWork.SemesterRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo Semester thất bại");

        }

        public async Task<bool> DeleteSemester(Guid semesterId)
        {
            var result = await _unitOfWork.SemesterRepository.GetByIdAsync(semesterId);

            if (result == null)
                throw new Exception("Không tìm thấy Semester này");
            else
            {
                _unitOfWork.SemesterRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa Semester thất bại");
            }
        }

        public async Task<Semester> GetSemesterById(Guid semesterId)
        {
            var semester = await _unitOfWork.SemesterRepository.GetByIdAsync(semesterId);
            return semester;
        }

        public async Task<List<Semester>> GetSemesters()
        {
            var semesters = _unitOfWork.SemesterRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).ToList();
            return semesters;
        }

        public async Task<bool> UpdateSemester(UpdateSemesterViewModel updateSemesterViewModel)
        {
            var validator = new UpdateSemesterViewModelValidator();
            var validationResult = validator.Validate(updateSemesterViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var semester = await _unitOfWork.SemesterRepository.GetSemesterBySemesterName(updateSemesterViewModel.SemesterName);
            if (!semester.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }
            
            semester = await _unitOfWork.SemesterRepository.GetSemesterByStartDate(updateSemesterViewModel.StartDate);
            if (!semester.IsNullOrEmpty())
            {
                throw new Exception("Ngày bắt đầu đã tồn tại");
            }

            var mapper = _mapper.Map<Semester>(updateSemesterViewModel);
            _unitOfWork.SemesterRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật Semester thất bại");
        }
    }
}
