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
        private readonly ISemesterCourseRepository _semesterCourseRepository;
        private readonly IClassRepository _classRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogEquipmentRepository _logEquipmentRepository;

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository, ITagRepository tagRepository,
            ILocationRepository locationRepository, ICategoryEquipmentRepository categoryEquipmentRepository
            , ISemesterRepository semesterRepository, IRoomRepository roomRepository, IEquipmentRepository equipmentRepository
            , IBlogRepository blogRepository
            , IChildrenRepository childrenRepository, INotificationRepository notificationRepository
            , INotificationUserRepository notificationUserRepository, IRatingRepository ratingRepository, IDivisionRepository divisionRepository
            , ILessonRepository lessonRepository, IQuestionRepository questionRepository, IRequestRepository requestRepository,
            ICourseRepository courseRepository, ISemesterCourseRepository semesterCourseRepository, IClassRepository classRepository
            , IDocumentRepository documentRepository, ILogEquipmentRepository logEquipmentRepository)
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
            _semesterCourseRepository = semesterCourseRepository;
            _classRepository = classRepository;
            _documentRepository = documentRepository;
            _logEquipmentRepository = logEquipmentRepository;
        }

        public IRoleRepository RoleRepository => _roleRepository;
        public IUserRepository UserRepository => _userRepository;

        public ITagRepository TagRepository => _tagRepository;

        public ILocationRepository LocationRepository => _locationRepository;

        public ICategoryEquipmentRepository CategoryEquipmentRepository => _categoryEquipmentRepository;

        public ISemesterRepository SemesterRepository => _semesterRepository;

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

        public ISemesterCourseRepository SemesterCourseRepository => _semesterCourseRepository;
        public IRequestRepository RequestRepository => _requestRepository;

        public IQuestionRepository QuestionRepository => _questionRepository;

        public IClassRepository ClassRepository => _classRepository;

        public IDocumentRepository DocumentRepository => _documentRepository;

        public ILogEquipmentRepository LogEquipmentRepository => _logEquipmentRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
