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

    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _dbContext;
        public NotificationRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Notification>> GetNotificationsByUserId(Guid userId)
        {
            var notifications = await _dbContext.Notification
            .Include(n => n.NotificationUser) 
            .Where(n => n.NotificationUser.Any(u => u.UserId == userId))
            .ToListAsync();

            return notifications;
        }
    }
}
