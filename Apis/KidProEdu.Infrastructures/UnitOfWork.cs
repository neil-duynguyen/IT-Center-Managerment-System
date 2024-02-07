using KidProEdu.Application;
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

        public UnitOfWork(AppDbContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository, ITagRepository tagRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tagRepository = tagRepository;
        }

        public IRoleRepository RoleRepository => _roleRepository;
        public IUserRepository UserRepository => _userRepository;

        public ITagRepository TagRepository => _tagRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
