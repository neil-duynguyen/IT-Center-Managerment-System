using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.IRepositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetTransactionByMonthInYear(DateTime monthInYear);
        Task<List<Transaction>> GetTransactionByYear(DateTime monthInYear);
        Task<List<Transaction>> GetTransactionByCourse(DateTime monthInYear);
        Task<double> GetTransactionsTotalAmount (DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetTransactionsByMonth(DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetTransactionsByCourse(DateTime startDate, DateTime endDate);
    }
}
