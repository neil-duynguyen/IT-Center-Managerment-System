using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EquipmentViewModels
{

    public class EquipmentRepairManagementViewModel
    {
        public Guid EquipmentIdId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? UserAccountId { get; set; }
        public DateTime? ReturnedDealine { get; set; }
    }
}
