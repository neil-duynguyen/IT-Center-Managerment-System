using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Discount : BaseEntity
    {
        public Guid UserId { get; set; }
        public string DiscountName { get; set; }
        public string DiscountType { get; set; }
        public string DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusDiscount Status { get; set; } = StatusDiscount.Active;
        public string Description { get; set; }
        public virtual User User { get; set; }

    }
}
