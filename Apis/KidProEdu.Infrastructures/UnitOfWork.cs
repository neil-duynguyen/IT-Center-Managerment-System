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
        private readonly ISemesterRepository _semesterRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ITrainingProgramCategoryRepository _trainingProgramCategoryRepository;
        private readonly IBlogRepository _blogRepository;

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository, ITagRepository tagRepository,
            ILocationRepository locationRepository, ICategoryEquipmentRepository categoryEquipmentRepository
            , ISemesterRepository semesterRepository, IRoomRepository roomRepository, IEquipmentRepository equipmentRepository
            , ITrainingProgramCategoryRepository trainingProgramCategoryRepository, IBlogRepository blogRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tagRepository = tagRepository;
            _locationRepository = locationRepository;
            _categoryEquipmentRepository = categoryEquipmentRepository;
            _semesterRepository = semesterRepository;
            _roomRepository = roomRepository;
            _equipmentRepository = equipmentRepository;
            _trainingProgramCategoryRepository = trainingProgramCategoryRepository;
            _blogRepository = blogRepository;
        }

        public IRoleRepository RoleRepository => _roleRepository;
        public IUserRepository UserRepository => _userRepository;

        public ITagRepository TagRepository => _tagRepository;

        public ILocationRepository LocationRepository => _locationRepository;

        public ICategoryEquipmentRepository CategoryEquipmentRepository => _categoryEquipmentRepository;

        public ISemesterRepository SemesterRepository => _semesterRepository;

        public IRoomRepository RoomRepository => _roomRepository;

        public IEquipmentRepository EquipmentRepository => _equipmentRepository;

        public ITrainingProgramCategoryRepository TrainingProgramCategoryRepository => _trainingProgramCategoryRepository;

        public IBlogRepository BlogRepository => _blogRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
