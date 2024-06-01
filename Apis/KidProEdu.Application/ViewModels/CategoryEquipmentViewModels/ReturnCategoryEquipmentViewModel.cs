using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CategoryEquipmentViewModels
{
    public class ReturnCategoryEquipmentViewModel
    {
        public Guid CategoryEquipmentId { get; set; }
        public int Quantity { get; set; }
        public Guid? UserAccountId { get; set; }
        public string Note { get; set; }

    }
}
