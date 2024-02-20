﻿
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class UserRepository : GenericRepository<UserAccount>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public Task<bool> CheckUserNameExited(string username) => _dbContext.Users.AnyAsync(u => u.UserName == username);

        public async Task<UserAccount> GetUserByUserNameAndPasswordHash(string username, string passwordHash)
        {
            var user = await _dbContext.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(record => record.UserName == username
                                        && record.PasswordHash == passwordHash);
            if (user is null)
            {
                throw new Exception("UserName & password is not correct");
            }
            return user;
        }

        public override async Task<List<UserAccount>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Role).Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<UserAccount> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Role).Where(x => !x.IsDeleted).FirstAsync(x => x.Id == id);
        }
    }
}
