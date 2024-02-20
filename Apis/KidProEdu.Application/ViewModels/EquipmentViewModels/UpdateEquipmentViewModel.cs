using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EquipmentViewModels
{
    public class UpdateEquipmentViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryEquipmentId { get; set; }
        public Guid? RoomId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double? Price { get; set; }
        public StatusOfEquipment? Status { get; set; }
        public DateTime? WarrantyDate { get; set; }
    }
}
