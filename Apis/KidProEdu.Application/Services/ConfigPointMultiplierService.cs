using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Services
{
    public class ConfigPointMultiplierService : IConfigPointMultiplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public ConfigPointMultiplierService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<List<ConfigPointMultiplier>> GetConfigPointMultipliers()
        {
            var results = _unitOfWork.ConfigPointMultiplierRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            return results;
        }

        public async Task<bool> UpdateConfigPointMultiplier(Guid id, double multiplier)
        {
            var configPointMultiplier = await _unitOfWork.ConfigPointMultiplierRepository.GetByIdAsync(id);
            if (configPointMultiplier == null)
            {
                throw new Exception("Không tìm thấy loại cấu hình");
            }
            configPointMultiplier.Multiplier = multiplier;

            _unitOfWork.ConfigPointMultiplierRepository.Update(configPointMultiplier);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật cấu hình thất bại");
        }
    }
}
