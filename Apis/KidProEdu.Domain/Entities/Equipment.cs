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
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        [ForeignKey("Class")]
        public Guid ClassId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double? Price { get; set; }
        public StatusOfEquipment? Status {  get; set; }
        public DateTime? WarrantyDate { get; set; }

        public virtual Order Order { get; set; }
        public virtual CategoryEquipment CategoryEquipment { get; set; }
        public virtual Class Class { get; set; }
    }
}
