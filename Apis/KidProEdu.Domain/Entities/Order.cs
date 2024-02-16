using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Order : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public StatusPayment PaymentStatus { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
        public IList<Transaction> Transactions { get; set; }
    }
}
