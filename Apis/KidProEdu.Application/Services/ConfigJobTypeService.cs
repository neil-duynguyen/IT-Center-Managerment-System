using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.ConfigJobTypes;
using KidProEdu.Application.ViewModels.ConfigJobType;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Services
{
    public class ConfigJobTypeService : IConfigJobTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ConfigJobTypeService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<List<ConfigJobTypeViewModel>> GetConfigJobTypes()
        {
            var results = _unitOfWork.ConfigJobTypeRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<ConfigJobTypeViewModel>>(results);
            return mapper;
        }

        public async Task<bool> UpdateConfigJobType(UpdateConfigJobTypeViewModel updateConfigJobTypeViewModel)
        {
            var validator = new UpdateConfigJobTypeViewModelValidator();
            var validationResult = validator.Validate(updateConfigJobTypeViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var configJobType = await _unitOfWork.ConfigJobTypeRepository.GetByIdAsync(updateConfigJobTypeViewModel.Id);
            if (configJobType == null)
            {
                throw new Exception("Không tìm thấy loại cấu hình");
            }

            _unitOfWork.ConfigJobTypeRepository.Update(configJobType);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật cấu hình thất bại");
        }
    }
}
