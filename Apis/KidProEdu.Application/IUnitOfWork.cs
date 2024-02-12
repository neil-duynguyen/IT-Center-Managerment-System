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
        public IChildrenRepository ChildrenRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
