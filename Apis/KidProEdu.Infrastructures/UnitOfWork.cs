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
        private readonly IChildrenRepository _childrenRepository;
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationUserRepository _notificationUserRepository;
        private readonly IRatingRepository _ratingRepository;

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository, ITagRepository tagRepository,
            ILocationRepository locationRepository, ICategoryEquipmentRepository categoryEquipmentRepository
            , ISemesterRepository semesterRepository, IRoomRepository roomRepository, IEquipmentRepository equipmentRepository
            , ITrainingProgramCategoryRepository trainingProgramCategoryRepository, IBlogRepository blogRepository
            , IChildrenRepository childrenRepository, ITrainingProgramRepository trainingProgramRepository, INotificationRepository notificationRepository
            , INotificationUserRepository notificationUserRepository, IRatingRepository ratingRepository)
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
            _childrenRepository = childrenRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _notificationRepository = notificationRepository;
            _notificationUserRepository = notificationUserRepository;
            _ratingRepository = ratingRepository;
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

        public IChildrenRepository ChildrenRepository => _childrenRepository;

        public ITrainingProgramRepository TrainingProgramRepository => _trainingProgramRepository;

        public INotificationRepository NotificationRepository => _notificationRepository;

        public INotificationUserRepository NotificationUserRepository => _notificationUserRepository;

        public IRatingRepository RatingRepository => _ratingRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
