using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TransactionViewModels
{
    public class TransactionSummaryByMonthInYearViewModel
    {
        public List<TransactionViewModel> Transactions { get; set; }
        public double TotalAmount { get; set; }
        public double TotalAmountOfMonthInYear { get; set; }

    }
}
