using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TransactionViewModels
{
    public class TransactionSummaryByMonthInYearViewModel
    {
        public string Month { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
        public double TotalAmountByYear { get; set; }
        public double TotalAmountOfMonthInYear { get; set; }
        public double Percent {  get; set; }

    }
}
