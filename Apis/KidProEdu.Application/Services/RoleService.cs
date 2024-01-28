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

        public RoleService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
        }

        public Task<bool> CreateRole(string roleView)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Role>> GetRole()
        {
            var role = await _unitOfWork.RoleRepository.GetAllAsync();

            //var mapper = _mapper.Map<List<RoleViewModel>>(role);

            return role;
        }
    }
}
