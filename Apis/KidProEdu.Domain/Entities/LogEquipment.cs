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
        public Guid? EquipmentId { get; set; }
        [ForeignKey("CategoryEquipment")]
        public Guid? CategoryEquipmentId { get; set; }
        [ForeignKey("UserAccount")]
        public Guid? UserAccountId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double? Price { get; set; }
        public StatusOfEquipment? Status { get; set; }
        public DateTime? RepairDate { get; set; }
        public string? WarrantyPeriod { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ReturnedDealine { get; set; }
        public Guid? RoomId { get; set; }
        public int? Quantity { get; set; }
        public string? Note { get; set; }
        public LogType? LogType { get; set; }
        public virtual Equipment? Equipment { get; set; }
        public virtual CategoryEquipment? CategoryEquipment { get; set; }
        public virtual UserAccount? UserAccount { get; set; }
    }
}
