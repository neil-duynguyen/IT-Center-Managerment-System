using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Requests;
using KidProEdu.Application.ViewModels.RequestViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace KidProEdu.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public RequestService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateRequest(CreateRequestViewModel createRequestViewModel)
        {
            var validator = new CreateRequestViewModelValidator();
            var validationResult = validator.Validate(createRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            /*var request = await _unitOfWork.RequestRepository.GetRequestByRequestType(createRequestViewModel.RequestType);
            if (!request.IsNullOrEmpty())
            {
                throw new Exception("Yêu cầu đã tồn tại");
            }*/

            var mapper = _mapper.Map<Request>(createRequestViewModel);

            List<Guid> guids = SplitGuid.SplitGuids(createRequestViewModel.UserIds);
            mapper.Status = Domain.Enums.StatusOfRequest.Pending;

            await _unitOfWork.RequestRepository.AddAsync(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo yêu cầu thất bại");

        }

        public async Task<bool> DeleteRequest(Guid requestId)
        {
            var result = await _unitOfWork.RequestRepository.GetByIdAsync(requestId);

            if (result == null)
                throw new Exception("Không tìm thấy yêu cầu này");
            else
            {
                _unitOfWork.RequestRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa yêu cầu thất bại");
            }
        }

        public async Task<Request> GetRequestById(Guid RequestId)
        {
            var request = await _unitOfWork.RequestRepository.GetByIdAsync(RequestId);
            return request;
        }

        public async Task<List<Request>> GetRequests()
        {
            var requests = _unitOfWork.RequestRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return requests;
        }

        public async Task<bool> UpdateRequest(UpdateRequestViewModel updateRequestViewModel)
        {
            var validator = new UpdateRequestViewModelValidator();
            var validationResult = validator.Validate(updateRequestViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            var request = await _unitOfWork.RequestRepository.GetByIdAsync(updateRequestViewModel.Id);
            if (request == null)
            {
                throw new Exception("Không tìm thấy yêu cầu");
            }

            var existingRequest = await _unitOfWork.RequestRepository.GetRequestByRequestType(updateRequestViewModel.RequestType);
            if (!existingRequest.IsNullOrEmpty())
            {
                if (existingRequest.FirstOrDefault().Id != updateRequestViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }

            /*Request.RequestName = updateRequestViewModel.RequestName;
            Request.Description = updateRequestViewModel.Description;*/

            _unitOfWork.RequestRepository.Update(request);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật yêu cầu thất bại");
        }
    }
}
