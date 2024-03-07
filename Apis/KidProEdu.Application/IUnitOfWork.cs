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
        public ISemesterRepository SemesterRepository { get; }
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
        public ISemesterCourseRepository SemesterCourseRepository { get; }
        public IClassRepository ClassRepository { get; }
        public IRequestUserAccountRepository RequestUserAccountRepository { get; }
        public IDocumentRepository DocumentRepository { get; }
        public ILogEquipmentRepository LogEquipmentRepository { get; }
        public IAdviseRequestRepository  AdviseRequestRepository { get; }
        public IDivisionUserAccountRepository DivisionUserAccountRepository { get; }
        public IEnrollmentRepository  EnrollmentRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
