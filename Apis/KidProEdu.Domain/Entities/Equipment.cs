using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Equipment : BaseEntity 
    {
        [ForeignKey("CategoryEquipment")]
        public Guid CategoryEquipmentId { get; set; }
        [ForeignKey("Room")]
        public Guid? RoomId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double? Price { get; set; }
        public StatusOfEquipment? Status {  get; set; }    
        public string? WarrantyPeriod { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public virtual CategoryEquipment CategoryEquipment { get; set; }
        public virtual Room? Room { get; set; }
        public IList<LogEquipment> LogEquipments { get; set; }       
    }
}
