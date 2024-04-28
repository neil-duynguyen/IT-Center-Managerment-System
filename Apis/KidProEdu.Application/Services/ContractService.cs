using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Contracts;
using KidProEdu.Application.ViewModels.ContractViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ContractService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }
        public async Task<bool> CreateContract(CreateContractViewModel createContractViewModel, Guid userId)
        {
            var validator = new CreateContractViewModelValidator();
            var validationResult = validator.Validate(createContractViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var contract = await _unitOfWork.ContractRepository.GetContractByCode(createContractViewModel.ContractCode);
            if (!contract.IsNullOrEmpty())
            {
                throw new Exception("Hợp đồng đã tồn tại");
            }

            var mapper = _mapper.Map<Contract>(createContractViewModel);
            mapper.UserId = userId;

            await _unitOfWork.ContractRepository.AddAsync(mapper);
            return true;
        }

        public async Task<bool> DeleteContract(Guid ContractId)
        {
            var result = await _unitOfWork.ContractRepository.GetByIdAsync(ContractId);

            if (result == null)
                throw new Exception("Không tìm thấy hợp đồng này");
            else
            {
                _unitOfWork.ContractRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa hợp đồng thất bại");
            }
        }

        public async Task<ContractViewModel> GetContractById(Guid ContractId)
        {
            var result = await _unitOfWork.ContractRepository.GetByIdAsync(ContractId);
            var mapper = _mapper.Map<ContractViewModel>(result);
            return mapper;
        }

        public async Task<List<ContractViewModel>> GetContracts()
        {
            var results = _unitOfWork.ContractRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<ContractViewModel>>(results);
            return mapper;
        }

        public async Task<bool> UpdateContract(UpdateContractViewModel updateContractViewModel)
        {
            var validator = new UpdateContractViewModelValidator();
            var validationResult = validator.Validate(updateContractViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var contract = await _unitOfWork.ContractRepository.GetByIdAsync(updateContractViewModel.Id);
            if (contract == null)
            {
                throw new Exception("Không tìm thấy hợp đồng");
            }

            /*var existingContract = await _unitOfWork.ContractRepository.GetContractByCode(updateContractViewModel.ContractCode);
            if (!existingContract.IsNullOrEmpty())
            {
                if (existingContract.FirstOrDefault().Id != updateContractViewModel.Id)
                {
                    throw new Exception("Hợp đồng đã tồn tại");
                }
            }*/

            /*Contract.Name = updateContractViewModel.Name;
            Contract.Status = updateContractViewModel.Status;*/

            _unitOfWork.ContractRepository.Update(contract);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật hợp đồng thất bại");
        }

        public async Task<List<ContractViewModel>> GetContractByTeacherId(Guid teacherId)
        {
            var result = await _unitOfWork.ContractRepository.GetContractByTeacherId(teacherId);
            var mapper = _mapper.Map<List<ContractViewModel>>(result);
            return mapper;
        }
    }
}
