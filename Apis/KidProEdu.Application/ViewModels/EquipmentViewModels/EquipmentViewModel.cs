using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EquipmentViewModels
{
    public class EquipmentViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryEquipmentId { get; set; }
        public Guid? RoomId { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
        public string? WarrantyPeriod { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }

    public class PrepareEquipmentViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
    }
}
