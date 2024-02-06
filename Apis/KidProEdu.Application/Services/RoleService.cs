using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.RoleViewModels;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public Task<bool> CreateRole(string roleView)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RoleViewModel>> GetRole()
        {
            var role = _unitOfWork.RoleRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false);

            return _mapper.Map<List<RoleViewModel>>(role);
        }
    }
}
