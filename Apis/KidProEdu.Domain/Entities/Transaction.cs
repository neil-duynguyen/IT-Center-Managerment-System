using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public string BankingAccountNumber { get; set; }
        public string BankingNumber { get; set; }
        public string BankName { get; set; }
        public double Amount { get; set; }
        public string CourseName { get; set; }
        public string Message { get; set; }
        public PayType PayType { get; set; }
        public DateTime PayDate { get; set; }
        public int InstallmentTerm { get; set; }
        public DateTime? InstallmentPeriod { get; set; }
        public StatusTransaction StatusTransaction { get; set; }
        public Guid ParentsTransaction { get; set; }
        public virtual Order Order { get; set; }
    }
}
