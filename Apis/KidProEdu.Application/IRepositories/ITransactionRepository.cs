using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;

namespace KidProEdu.Application.IRepositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetTransactionByMonthInYear(DateTime monthInYear);
        Task<List<Transaction>> GetTransactionByYear(DateTime monthInYear);
    }
}
