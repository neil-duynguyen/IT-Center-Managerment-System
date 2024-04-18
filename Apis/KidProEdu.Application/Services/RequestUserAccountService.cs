using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.RequestUserAccounts;
using KidProEdu.Application.ViewModels.RequestUserAccountViewModels;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Services
{
    public class RequestUserAccountService : IRequestUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public RequestUserAccountService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateRequestUserAccount(CreateRequestUserAccountViewModel createRequestUserAccountViewModel)
        {
            try
            {
                var validator = new CreateRequestUserAccountViewModelValidator();
                var validationResult = validator.Validate(createRequestUserAccountViewModel);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }

                if (createRequestUserAccountViewModel.RecieverIds.Length != 0)
                {
                    foreach (var item in createRequestUserAccountViewModel.RecieverIds)
                    {
                        RequestUserAccount re = new()
                        {
                            RecieverId = item,
                            RequestId = createRequestUserAccountViewModel.RequestId,
                            //Status = Domain.Enums.StatusOfRequest.Pending
                        };

                        await _unitOfWork.RequestUserAccountRepository.AddAsync(re);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Tạo yêu cầu đến người nhận thất bại");
            }


        }
    }
}
