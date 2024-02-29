using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class LogEquipment : BaseEntity
    {
        [ForeignKey("Equipment")]
        public Guid EquipmentId { get; set; }
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double? Price { get; set; }
        public StatusOfEquipment? Status { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public DateTime? WarrantyPeriod { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public Guid? RoomId { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
