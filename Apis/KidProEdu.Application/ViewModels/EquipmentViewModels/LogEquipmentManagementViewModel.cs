using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EquipmentViewModels
{
    public class LogEquipmentManagementViewModel
    {
        public Guid? UserAccountId { get; set; }
        public DateTime? RepairDate { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ReturnedDealine { get; set; }
        public Guid? RoomId { get; set; }
    }
}
