using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.LogEquipmentViewModels
{

    public class UpdateLogEquipmentViewModel
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid UserAccountId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double? Price { get; set; }
        public StatusOfEquipment? Status { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public DateTime? WarrantyPeriod { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public Guid? RoomId { get; set; }
    }
}
