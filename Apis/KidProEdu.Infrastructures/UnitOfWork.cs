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
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IConfigJobTypeRepository _configJobTypeRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ISkillCertificateRepository _skillCertificateRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ITeachingClassHistoryRepository _teachingClassHistoryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IScheduleRoomRepository _scheduleRoomRepository;
        private readonly IExamRepository _examRepository;
        private readonly IChildrenAnswerRepository _childrenAnswerRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IConfigPointMultiplierRepository _configPointMultiplierRepository;
        private readonly ICertificateRepository _certificateRepository;

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
            ISkillRepository skillRepository, ISkillCertificateRepository skillCertificateRepository, IAttendanceRepository attendanceRepository,
            IScheduleRepository scheduleRepository, ISlotRepository slotRepository,
            IContractRepository contractRepository, IConfigJobTypeRepository configJobTypeRepository, 
            ITeachingClassHistoryRepository teachingClassHistoryRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository,
            ITransactionRepository transactionRepository, IScheduleRoomRepository scheduleRoomRepository,
            IExamRepository examRepository, IFeedbackRepository feedbackRepository,
            IChildrenAnswerRepository childrenAnswerRepository, IConfigPointMultiplierRepository configPointMultiplierRepository, ICertificateRepository certificateRepository
            )
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
            _scheduleRepository = scheduleRepository;
            _slotRepository = slotRepository;
            _contractRepository = contractRepository;
            _configJobTypeRepository = configJobTypeRepository;
            _skillRepository = skillRepository;
            _skillCertificateRepository = skillCertificateRepository;
            _attendanceRepository = attendanceRepository;
            _teachingClassHistoryRepository = teachingClassHistoryRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository; 
            _transactionRepository = transactionRepository;
            _scheduleRoomRepository = scheduleRoomRepository;
            _examRepository = examRepository;
            _childrenAnswerRepository = childrenAnswerRepository;
            _feedbackRepository = feedbackRepository;
            _configPointMultiplierRepository = configPointMultiplierRepository;
            _certificateRepository = certificateRepository;
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

        public IScheduleRepository ScheduleRepository => _scheduleRepository;

        public ISlotRepository SlotRepository => _slotRepository;

        public IContractRepository ContractRepository => _contractRepository;

        public IConfigJobTypeRepository ConfigJobTypeRepository => _configJobTypeRepository;

        public ISkillRepository SkillRepository => _skillRepository;

        public ISkillCertificateRepository SkillCertificateRepository => _skillCertificateRepository;

        public IAttendanceRepository AttendanceRepository => _attendanceRepository;
        public ITeachingClassHistoryRepository TeachingClassHistoryRepository => _teachingClassHistoryRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;

        public ITransactionRepository TransactionRepository => _transactionRepository;
        public IScheduleRoomRepository ScheduleRoomRepository => _scheduleRoomRepository;

        public IExamRepository ExamRepository => _examRepository;

        public IChildrenAnswerRepository ChildrenAnswerRepository => _childrenAnswerRepository;

        public IFeedbackRepository FeedbackRepository => _feedbackRepository;

        public IConfigPointMultiplierRepository ConfigPointMultiplierRepository => _configPointMultiplierRepository;
        public ICertificateRepository CertificateRepository => _certificateRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
