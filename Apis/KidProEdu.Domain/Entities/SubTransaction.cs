using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class SubTransaction : BaseEntity
    {
        [ForeignKey("Transaction")]
        public Guid TransactionId { get; set; }
        public string Name { get; set; }
        public string BankingAccountNumber { get; set; }
        public string BankingNumber { get; set; }
        public string BankName { get; set; }
        public double Amount { get; set; }
        public string CourseName { get; set; }
        public string Message { get; set; }
        public string PayType { get; set; }
        public DateTime PayDate { get; set; }
        public StatusSubTransaction StatusSubTransaction { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
