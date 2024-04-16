using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Divisions;
using KidProEdu.Application.Validations.Ratings;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class DivisionService : IDivisionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public DivisionService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateDivision(CreateDivisionViewModel createDivisionViewModel)
        {
            var validator = new CreateDivisionViewModelValidator();
            var validationResult = validator.Validate(createDivisionViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var division = await _unitOfWork.DivisionRepository.GetDivisionByName(createDivisionViewModel.Name);
            if (!division.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }
            var mapper = _mapper.Map<Division>(createDivisionViewModel);
            await _unitOfWork.DivisionRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo phòng ban thất bại");
        }

        public async Task<bool> DeleteDivision(Guid id)
        {
            var result = await _unitOfWork.DivisionRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy phòng ban này");
            else
            {
                _unitOfWork.DivisionRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa phòng ban thất bại");
            }
        }

        public async Task<DivisionViewModel> GetDivisionById(Guid id)
        {
            var results = await _unitOfWork.DivisionRepository.GetByIdAsync(id);

            var mapper = _mapper.Map<DivisionViewModel>(results);

            return mapper;
        }

        public async Task<List<DivisionViewModel>> GetDivisions()
        {
            var results = _unitOfWork.DivisionRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var mapper = _mapper.Map<List<DivisionViewModel>>(results);

            return mapper;
        }

        public async Task<bool> UpdateDivision(UpdateDivisionViewModel updateDivisionViewModel)
        {
            var validator = new UpdateDivisionViewModelValidator();
            var validationResult = validator.Validate(updateDivisionViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var division = await _unitOfWork.DivisionRepository.GetByIdAsync(updateDivisionViewModel.Id);
            if (division == null)
            {
                throw new Exception("Không tìm thấy phòng ban");
            }


            var existingDivision = await _unitOfWork.DivisionRepository.GetDivisionByName(updateDivisionViewModel.Name);
            if (!existingDivision.IsNullOrEmpty())
            {
                if (existingDivision.FirstOrDefault().Id != updateDivisionViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }
            division.Name = updateDivisionViewModel.Name;
            division.Description = updateDivisionViewModel.Description;   
            _unitOfWork.DivisionRepository.Update(division);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật phòng ban thất bại");
        }
    }
}
