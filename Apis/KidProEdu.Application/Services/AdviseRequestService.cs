using AutoMapper;
using AutoMapper.Execution;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.AdviseRequests;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

namespace KidProEdu.Application.Services
{
    public class AdviseRequestService : IAdviseRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public AdviseRequestService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }
        public async Task<bool> CreateAdviseRequest(CreateAdviseRequestViewModel createAdviseRequestViewModel)
        {
            var validator = new CreateAdviseRequestViewModelValidator();
            var validationResult = validator.Validate(createAdviseRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var adviseRequest = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByEmail(createAdviseRequestViewModel.Email);
            if (adviseRequest != null)
            {
                throw new Exception("Email đã tồn tại");
            }

            adviseRequest = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByPhone(createAdviseRequestViewModel.Phone);
            if (adviseRequest != null)
            {
                throw new Exception("Phone đã tồn tại");
            }

            var mapper = _mapper.Map<AdviseRequest>(createAdviseRequestViewModel);
            mapper.StatusAdviseRequest = Domain.Enums.StatusAdviseRequest.Pending;

            await _unitOfWork.AdviseRequestRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo yêu cầu tư vấn thất bại");
        }

        public async Task<bool> DeleteAdviseRequest(Guid AdviseRequestId)
        {
            var result = await _unitOfWork.AdviseRequestRepository.GetByIdAsync(AdviseRequestId);

            if (result == null)
                throw new Exception("Không tìm thấy yêu cầu tư vấn này");
            else
            {
                _unitOfWork.AdviseRequestRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa yêu cầu tư vấn thất bại");
            }
        }

        public async Task<AdviseRequestViewModel> GetAdviseRequestById(Guid AdviseRequestId)
        {
            var result = await _unitOfWork.AdviseRequestRepository.GetByIdAsync(AdviseRequestId);
            var mapper = _mapper.Map<AdviseRequestViewModel>(result);
            return mapper;
        }

        public async Task<List<AdviseRequestViewModel>> GetAdviseRequests()
        {
            var results = _unitOfWork.AdviseRequestRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<AdviseRequestViewModel>>(results);
            return mapper;
        }

        public async Task<bool> UpdateAdviseRequest(UpdateAdviseRequestViewModel updateAdviseRequestViewModel, params Expression<Func<AdviseRequest, object>>[] uniqueProperties)
        {
            var validator = new UpdateAdviseRequestViewModelValidator();
            var validationResult = validator.Validate(updateAdviseRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var adviseRequest = await _unitOfWork.AdviseRequestRepository.GetByIdAsync(updateAdviseRequestViewModel.Id)
                ?? throw new Exception("Không tìm thấy yêu cầu tư vấn");
            if (adviseRequest.UserId != null)
            {
                throw new Exception("Yêu cầu tư vấn này đã có người nhận");
            }

            foreach (var property in uniqueProperties)
            {
                var obj = await _unitOfWork.AdviseRequestRepository.GetAdviseRequestByProperty(updateAdviseRequestViewModel, property);
                if (obj != null && obj.Id != updateAdviseRequestViewModel.Id)
                {
                    throw new Exception($"{property.GetMember().Name} đã tồn tại");
                }
            }

            var mapper = _mapper.Map(updateAdviseRequestViewModel, adviseRequest);

            _unitOfWork.AdviseRequestRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật yêu cầu tư vấn thất bại");
        }
    }
}
