using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EquipmentViewModels
{
    public class EquipmentManagementViewModel
    {
        public Guid Id { get; set; }
        public Guid? RoomId { get; set; }
    }
}
