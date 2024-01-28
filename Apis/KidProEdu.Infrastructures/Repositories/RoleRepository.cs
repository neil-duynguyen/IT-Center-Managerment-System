using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Infrastructures.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }
    }

}
