using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EquipmentViewModels
{
    public class LogEquipmentBorrowedManagementViewModel
    {
        public Guid? UserAccountId { get; set; }
        public DateTime? ReturnedDealine { get; set; }
    }
}
