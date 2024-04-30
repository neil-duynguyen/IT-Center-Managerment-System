using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _dbContext;
        public TransactionRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }
        public override async Task<List<Transaction>> GetAllAsync()
        {
            return await _dbContext.Transaction.Include(x => x.OrderDetail).ThenInclude(x => x.Order).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionByCourse(DateTime monthInYear)
        {
            var transactions = await _dbContext.Transaction
                .Include(x => x.OrderDetail)
                .ThenInclude(x => x.Course)
                .Where(x => x.StatusTransaction == StatusTransaction.Successfully && x.PayDate.Value.Year == monthInYear.Year && !x.IsDeleted).ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetTransactionByMonthInYear(DateTime monthInYear)
        {
            var transactions = await _dbContext.Transaction
                .Include(x => x.OrderDetail)
                    .ThenInclude(x => x.Order)
                .Where(x => x.StatusTransaction == StatusTransaction.Successfully && x.PayDate.Value.Month == monthInYear.Month && x.PayDate.Value.Year == monthInYear.Year && !x.IsDeleted)
                .ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetTransactionByYear(DateTime monthInYear)
        {
            var transactions = await _dbContext.Transaction
                .Include(x => x.OrderDetail)
                .ThenInclude(x => x.Order)
                .Where(x => x.StatusTransaction == StatusTransaction.Successfully && x.PayDate.Value.Year == monthInYear.Year && !x.IsDeleted).ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetTransactionsByCourse(DateTime startDate, DateTime endDate)
        {
            var transactions = await _dbContext.Transaction
                .Include(x => x.OrderDetail)
                .ThenInclude(x => x.Course)
                .Where(x => x.StatusTransaction == StatusTransaction.Successfully && x.PayDate >= startDate && x.PayDate <= endDate && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetTransactionsByMonth(DateTime startDate, DateTime endDate)
        {
            var transactions = await _dbContext.Transaction
               .Where(x => x.StatusTransaction == StatusTransaction.Successfully && x.PayDate.Value.Date >= startDate.Date && x.PayDate.Value.Date <= endDate.Date && !x.IsDeleted)
               .AsNoTracking()
               .ToListAsync();
            return transactions;
        }

        public async Task<double> GetTransactionsTotalAmount(DateTime startDate, DateTime endDate)
        {
            var totalAmount = await _dbContext.Transaction
                .Where(x => x.StatusTransaction == StatusTransaction.Successfully && x.PayDate >= startDate && x.PayDate <= endDate && !x.IsDeleted)
                .AsNoTracking()
                .SumAsync(t => t.TotalAmount ?? 0);
            return totalAmount;
        }
    }
}
