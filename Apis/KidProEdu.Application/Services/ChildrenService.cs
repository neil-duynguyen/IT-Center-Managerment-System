using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Children;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class ChildrenService : IChildrenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ChildrenService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateChildren(CreateChildrenViewModel createChildrenViewModel)
        {
            var validator = new CreateChildrenViewModelValidator();
            var validatorResult = await validator.ValidateAsync(createChildrenViewModel);
            if (!validatorResult.IsValid)
            {
                foreach (var error in validatorResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var mapper = _mapper.Map<ChildrenProfile>(createChildrenViewModel);
            await _unitOfWork.ChildrenRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo trẻ thất bại.");
        }

        public async Task<bool> UpdateChildren(UpdateChildrenViewModel updateChildrenViewModel)
        {
            var validator = new UpdateChildrenViewModelValidator();
            var validatorResult = await validator.ValidateAsync(updateChildrenViewModel);
            if (!validatorResult.IsValid)
            {
                foreach (var error in validatorResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var mapper = _mapper.Map<ChildrenProfile>(updateChildrenViewModel);
            _unitOfWork.ChildrenRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật trẻ thất bại.");
        }

        public Task<bool> DeleteChildren(Guid childrenId)
        {
            throw new NotImplementedException();
        }

        public Task<Location> GetChildrenById(Guid childrenId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChildrenViewModel>> GetChildrensByStaffId()
        {
            var getChildrens = _unitOfWork.ChildrenRepository.GetAllAsync().Result.Where(x => x.CreatedBy == _claimsService.GetCurrentUserId).ToList();
            return _mapper.Map<List<ChildrenViewModel>>(getChildrens);
        }

    }
}
