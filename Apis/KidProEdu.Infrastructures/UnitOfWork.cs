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
        private readonly IRoomRepository _roomRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IChildrenRepository _childrenRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationUserRepository _notificationUserRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IClassRepository _classRepository;
        private readonly IRequestUserAccountRepository _requestUserAccountRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly ILogEquipmentRepository _logEquipmentRepository;
        private readonly IAdviseRequestRepository _adviseRequestRepository;
        private readonly IDivisionUserAccountRepository _divisionUserAccountRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IConfigJobTypeRepository _configJobTypeRepository;

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository, ITagRepository tagRepository,
            ILocationRepository locationRepository, ICategoryEquipmentRepository categoryEquipmentRepository
            , IRoomRepository roomRepository, IEquipmentRepository equipmentRepository
            , IBlogRepository blogRepository
            , IChildrenRepository childrenRepository, INotificationRepository notificationRepository
            , INotificationUserRepository notificationUserRepository, IRatingRepository ratingRepository, IDivisionRepository divisionRepository
            , ILessonRepository lessonRepository, IQuestionRepository questionRepository, IRequestRepository requestRepository,
            ICourseRepository courseRepository, IClassRepository classRepository
            , IRequestUserAccountRepository requestUserAccountRepository, IResourceRepository resourceRepository
            , ILogEquipmentRepository logEquipmentRepository, IAdviseRequestRepository adviseRequestRepository, 
            IDivisionUserAccountRepository divisionUserAccountRepository, IEnrollmentRepository enrollmentRepository, 
            IContractRepository contractRepository, IConfigJobTypeRepository configJobTypeRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tagRepository = tagRepository;
            _locationRepository = locationRepository;
            _categoryEquipmentRepository = categoryEquipmentRepository;
            _roomRepository = roomRepository;
            _equipmentRepository = equipmentRepository;
            _blogRepository = blogRepository;
            _childrenRepository = childrenRepository;
            _notificationRepository = notificationRepository;
            _notificationUserRepository = notificationUserRepository;
            _ratingRepository = ratingRepository;
            _divisionRepository = divisionRepository;
            _lessonRepository = lessonRepository;
            _questionRepository = questionRepository;
            _requestRepository = requestRepository;
            _courseRepository = courseRepository;
            _classRepository = classRepository;
            _requestUserAccountRepository = requestUserAccountRepository;
            _resourceRepository = resourceRepository;
            _logEquipmentRepository = logEquipmentRepository;
            _adviseRequestRepository = adviseRequestRepository;
            _divisionUserAccountRepository = divisionUserAccountRepository;
            _enrollmentRepository = enrollmentRepository;
            _contractRepository = contractRepository;
            _configJobTypeRepository = configJobTypeRepository;
        }

        public IRoleRepository RoleRepository => _roleRepository;
        public IUserRepository UserRepository => _userRepository;

        public ITagRepository TagRepository => _tagRepository;

        public ILocationRepository LocationRepository => _locationRepository;

        public ICategoryEquipmentRepository CategoryEquipmentRepository => _categoryEquipmentRepository;

        public IRoomRepository RoomRepository => _roomRepository;

        public IEquipmentRepository EquipmentRepository => _equipmentRepository;

        public IBlogRepository BlogRepository => _blogRepository;

        public IChildrenRepository ChildrenRepository => _childrenRepository;

        public INotificationRepository NotificationRepository => _notificationRepository;

        public INotificationUserRepository NotificationUserRepository => _notificationUserRepository;

        public IRatingRepository RatingRepository => _ratingRepository;

        public IDivisionRepository DivisionRepository => _divisionRepository;

        public ILessonRepository LessonRepository => _lessonRepository;

        public ICourseRepository CourseRepository => _courseRepository;

        public IRequestRepository RequestRepository => _requestRepository;

        public IQuestionRepository QuestionRepository => _questionRepository;

        public IClassRepository ClassRepository => _classRepository;

        public IRequestUserAccountRepository RequestUserAccountRepository => _requestUserAccountRepository;

        public IResourceRepository ResourceRepository => _resourceRepository;

        public ILogEquipmentRepository LogEquipmentRepository => _logEquipmentRepository;

        public IAdviseRequestRepository AdviseRequestRepository => _adviseRequestRepository;

        public IDivisionUserAccountRepository DivisionUserAccountRepository => _divisionUserAccountRepository;

        public IEnrollmentRepository EnrollmentRepository => _enrollmentRepository;

        public IContractRepository ContractRepository => _contractRepository;

        public IConfigJobTypeRepository ConfigJobTypeRepository => _configJobTypeRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
