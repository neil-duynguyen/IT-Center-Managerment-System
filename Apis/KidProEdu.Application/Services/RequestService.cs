using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.Requests;
using KidProEdu.Application.ViewModels.RequestUserAccountViewModels;
using KidProEdu.Application.ViewModels.RequestViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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

            //List<Guid> guids = SplitGuid.SplitGuids(createRequestViewModel.UserIds);

            //mapper.UserId = _claimsService.GetCurrentUserId;
            mapper.Status = Domain.Enums.StatusOfRequest.Pending;

            await _unitOfWork.RequestRepository.AddAsync(mapper);

            //tạo request user account
            CreateRequestUserAccountViewModel model = new()
            {
                RequestId = mapper.Id,
                RecieverIds = createRequestViewModel.UserIds
            };
            RequestUserAccountService service = new(_unitOfWork, _currentTime, _claimsService, _mapper);
            await service.CreateRequestUserAccount(model);

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

        public async Task<List<RequestViewModel>> GetRequests()
        {
            var requests = _unitOfWork.RequestRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<RequestViewModel>>(requests);
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

            var request = await _unitOfWork.RequestRepository.GetByIdAsync(updateRequestViewModel.Id)
                ?? throw new Exception("Không tìm thấy yêu cầu");

            /*var existingRequest = await _unitOfWork.RequestRepository.GetRequestByRequestType(updateRequestViewModel.RequestType);
            if (!existingRequest.IsNullOrEmpty())
            {
                if (existingRequest.FirstOrDefault().Id != updateRequestViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }*/

            if (request.Status != Domain.Enums.StatusOfRequest.Pending)
                throw new Exception("Cập nhật yêu cầu thất bại, yêu cầu này đã được xử lý");

            /*Request.RequestName = updateRequestViewModel.RequestName;
            Request.Description = updateRequestViewModel.Description;*/

            var mapper = _mapper.Map(updateRequestViewModel, request);

            _unitOfWork.RequestRepository.Update(mapper);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật yêu cầu thất bại");
        }

        public async Task<bool> ChangeStatusRequest(ChangeStatusRequestViewModel changeStatusRequestViewModel)
        {
            var status = Domain.Enums.StatusOfRequest.Pending;

            switch (changeStatusRequestViewModel.Status)
            {
                case "Approved":
                    status = Domain.Enums.StatusOfRequest.Approved;

                    foreach (var idRequest in changeStatusRequestViewModel.RequestIds)
                    {
                        var request = await _unitOfWork.RequestRepository.GetByIdAsync(idRequest);
                        switch (request.RequestType)
                        {
                            case "Location":
                                var user = await _unitOfWork.UserRepository.GetByIdAsync((Guid)request.CreatedBy);
                                //user
                                //thiếu id location bên user 
                                break;
                            case "Class":
                                var findClass = await _unitOfWork.ClassRepository.GetByIdAsync((Guid)request.ClassId);
                                // đổi được gv qua lớp kia nhưng k biết đổi từ lớp nào
                                break;
                            case "Schedule":
                                break;
                            case "Equipment":
                                break;
                            case "Leave":
                                break;
                            case "Refund":
                                break;
                            default:
                                //throw new Exception("Loại request không được hỗ trợ");
                                break;
                        }

                        request.Status = status;

                        _unitOfWork.RequestRepository.Update(request);
                    }

                    break;
                /*case "Pending":
                    status = Domain.Enums.StatusOfRequest.Pending;
                    break;*/
                case "Cancel":
                    status = Domain.Enums.StatusOfRequest.Cancel;
                    break;
                default:
                    throw new Exception("Trạng thái không được hỗ trợ");
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật trạng thái yêu cầu thất bại");
        }
    }
}
