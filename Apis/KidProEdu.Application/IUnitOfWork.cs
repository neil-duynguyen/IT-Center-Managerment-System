using KidProEdu.Application.IRepositories;
using KidProEdu.Application.Repositories;

namespace KidProEdu.Application
{
    public interface IUnitOfWork
    {
        public IRoleRepository RoleRepository { get; }
        public IUserRepository UserRepository { get; }
        public ITagRepository TagRepository { get; }
        public ILocationRepository LocationRepository { get; }
        public ICategoryEquipmentRepository CategoryEquipmentRepository { get; }
        public IRoomRepository RoomRepository { get; }
        public IEquipmentRepository EquipmentRepository { get; }
        public IBlogRepository BlogRepository { get; }
        public IChildrenRepository ChildrenRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public INotificationUserRepository NotificationUserRepository { get; }
        public IRatingRepository RatingRepository { get; }
        public IDivisionRepository DivisionRepository { get; }
        public ILessonRepository LessonRepository { get; }
        public IQuestionRepository QuestionRepository { get; }
        public IRequestRepository RequestRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IClassRepository ClassRepository { get; }
        public IRequestUserAccountRepository RequestUserAccountRepository { get; }
        public IResourceRepository ResourceRepository { get; }
        public ILogEquipmentRepository LogEquipmentRepository { get; }
        public IAdviseRequestRepository  AdviseRequestRepository { get; }
        public IDivisionUserAccountRepository DivisionUserAccountRepository { get; }
        public IEnrollmentRepository  EnrollmentRepository { get; }
        public IScheduleRepository  ScheduleRepository { get; }
        public ISlotRepository  SlotRepository { get; }
        public IContractRepository  ContractRepository { get; }
        public IConfigJobTypeRepository  ConfigJobTypeRepository { get; }
        public ISkillRepository SkillRepository { get; }
        public ISkillCertificateRepository SkillCertificateRepository { get; }
        public IAttendanceRepository AttendanceRepository { get; }
        public ITeachingClassHistoryRepository TeachingClassHistoryRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }    
        public ITransactionRepository TransactionRepository { get; }
        public IScheduleRoomRepository ScheduleRoomRepository { get; }
        public IExamRepository ExamRepository { get; }
        public IChildrenAnswerRepository ChildrenAnswerRepository { get; }
        public IFeedbackRepository FeedbackRepository { get; }
        public IConfigPointMultiplierRepository ConfigPointMultiplierRepository { get; }
        public ICertificateRepository CertificateRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
