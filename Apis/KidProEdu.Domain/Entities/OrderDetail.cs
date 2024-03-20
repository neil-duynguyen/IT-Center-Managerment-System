using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }
        [ForeignKey("ChildrenProfile")]
        public Guid? ChildrenProfileId { get; set; }
        public Guid? CourseId { get; set; }
        public int? Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double? TotalPrice { get; set; }
        public int? InstallmentTerm { get; set; }
        public PayType? PayType { get; set; }
        public Guid? ParentOrderDetail { get;set; } 
        public virtual Course? Course { get; set; }
        public virtual Order? Order { get; set; }
        public virtual ChildrenProfile? ChildrenProfile { get; set; }
        public IList<Transaction> Transactions { get; set; }
    }
}
