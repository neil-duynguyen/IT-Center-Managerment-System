using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Divisions;
using KidProEdu.Application.Validations.DivisionUserAccounts;
using KidProEdu.Application.ViewModels.DivisionUserAccountViewModels;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LogEquipmentViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class DivisionUserAccountService : IDivisionUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public DivisionUserAccountService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateDivisionUserAccount(CreateDivisionUserAccountViewModel createDivisionUserAccountViewModel)
        {
            var validator = new CreateDivisionUserAccountViewModelValidator();
            var validationResult = validator.Validate(createDivisionUserAccountViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var divisionUserAccount = await _unitOfWork.DivisionUserAccountRepository.GetDivisionUserAccountByUserId(createDivisionUserAccountViewModel.UserAccountId);
            if (divisionUserAccount != null)
            {
                throw new Exception("Người dùng đã tồn tại");
            }
            var mapper = _mapper.Map<DivisionUserAccount>(createDivisionUserAccountViewModel);
            await _unitOfWork.DivisionUserAccountRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo phòng ban cho người dùng thất bại");
        }

        public async Task<bool> DeleteDivisionUserAccount(Guid id)
        {
            var result = await _unitOfWork.DivisionUserAccountRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy phòng ban cho người dùng này");
            else
            {
                _unitOfWork.DivisionUserAccountRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa phòng ban cho người dùng thất bại");
            }
        }

        public async Task<DivisionUserAccountViewModel> GetDivisionUserAccountById(Guid id)
        {
            var results = await _unitOfWork.DivisionUserAccountRepository.GetByIdAsync(id);

            var mapper = _mapper.Map<DivisionUserAccountViewModel>(results);

            return mapper;
        }

        public async Task<DivisionUserAccountViewModel> GetDivisionUserAccountByUserId(Guid userId)
        {
            var results = await _unitOfWork.DivisionUserAccountRepository.GetDivisionUserAccountByUserId(userId);

            var mapper = _mapper.Map<DivisionUserAccountViewModel>(results);

            return mapper;
        }

        public async Task<List<DivisionUserAccountViewModel>> GetDivisionUserAccounts()
        {
            var results = _unitOfWork.DivisionUserAccountRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var mapper = _mapper.Map<List<DivisionUserAccountViewModel>>(results);

            return mapper;
        }

        public async Task<bool> UpdateDivisionUserAccount(UpdateDivisionUserAccountViewModel updateDivisionUserAccountViewModel)
        {
            var validator = new UpdateDivisionUserAccountViewModelValidator();
            var validationResult = validator.Validate(updateDivisionUserAccountViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var divisionUserAccount = await _unitOfWork.DivisionUserAccountRepository.GetByIdAsync(updateDivisionUserAccountViewModel.Id);
            if (divisionUserAccount == null)
            {
                throw new Exception("Không tìm thấy phòng ban cho người dùng");
            }

            var existingDivisionUserAccount = await _unitOfWork.DivisionUserAccountRepository.GetDivisionUserAccountByUserId(updateDivisionUserAccountViewModel.UserAccountId);
            if (existingDivisionUserAccount != null)
            {
                if (existingDivisionUserAccount.Id != updateDivisionUserAccountViewModel.Id)
                {
                    throw new Exception("Người dùng đã tồn tại");
                }
            }

            divisionUserAccount.UserAccountId = updateDivisionUserAccountViewModel.UserAccountId;
            divisionUserAccount.DivisionId = updateDivisionUserAccountViewModel.DivisionId;
            _unitOfWork.DivisionUserAccountRepository.Update(divisionUserAccount);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật phòng ban cho người dùng thất bại");
        }
    }
}
