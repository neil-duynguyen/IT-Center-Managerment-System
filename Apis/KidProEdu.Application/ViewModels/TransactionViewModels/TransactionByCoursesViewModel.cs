using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TransactionViewModels
{
    public class TransactionByCoursesViewModel
    {
        public List<TransactionViewModel> Transactions { get; set; }
        public string CourseName { get; set; }
        public double TotalAmountByYear { get; set; }
        public double TotalAmountByCourse { get; set; }
        public double Percent { get; set; }
    }
}
