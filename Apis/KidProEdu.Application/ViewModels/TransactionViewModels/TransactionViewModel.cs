using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TransactionViewModels
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
        public string? BankingAccountNumber { get; set; }
        public string? BankingNumber { get; set; }
        public string? BankName { get; set; }
        public string? CourseName { get; set; }
        public double? TotalAmount { get; set; }
        public string? Message { get; set; }
        public DateTime? PayDate { get; set; }
        public int? InstallmentTerm { get; set; }
        public DateTime? InstallmentPeriod { get; set; }
        public string? StatusTransaction { get; set; }
    }
}
