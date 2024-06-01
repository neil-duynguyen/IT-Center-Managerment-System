using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EquipmentViewModels
{

    public class EquipmentReturnedManagementViewModel
    {
        public Guid EquipmentId { get; set; }
        public Guid? UserAccountId { get; set; }
    }
}
