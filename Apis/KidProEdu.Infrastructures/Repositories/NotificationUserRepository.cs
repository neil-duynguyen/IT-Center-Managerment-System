using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{

    public class NotificationUserRepository : GenericRepository<NotificationUser>, INotificationUserRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationUserRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;

        }

        public async Task<List<NotificationUser>> GetListNotificationUserByUserId(Guid userId)
        {
            var notificationUser = await _dbContext.NotificationUser
                .Include(x=>x.Notification)
                .Where(x => x.UserId == userId).ToListAsync();

            return notificationUser;
        }
    }
}
