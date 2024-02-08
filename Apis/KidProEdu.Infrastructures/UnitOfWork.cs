using KidProEdu.Application;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.Repositories;
using KidProEdu.Infrastructures;
using KidProEdu.Infrastructures.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICategoryEquipmentRepository _categoryEquipmentRepository;


        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository, ITagRepository tagRepository,
            ILocationRepository locationRepository, ICategoryEquipmentRepository categoryEquipmentRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tagRepository = tagRepository;
            _locationRepository = locationRepository;
            _categoryEquipmentRepository = categoryEquipmentRepository;
        }

        public IRoleRepository RoleRepository => _roleRepository;
        public IUserRepository UserRepository => _userRepository;

        public ITagRepository TagRepository => _tagRepository;

        public ILocationRepository LocationRepository => _locationRepository;

        public ICategoryEquipmentRepository CategoryEquipmentRepository => _categoryEquipmentRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
