using KidProEdu.Application.ViewModels.TransactionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionViewModel>> GetAllTransaction();
        Task<List<TransactionViewModel>> GetTransactionDetailByTransactionId(Guid id);
        Task<TransactionSummaryViewModel> TransactionsSummarise();

        Task<List<TransactionSummaryByMonthInYearViewModel>> TransactionsSummariseByMonthInYear(DateTime monthInYear);

    }
}
