using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CategoryEquipmentViewModels
{
    public class BorrowForGoHomeCategoryEquipmentViewModel
    {
        public Guid CategoryEquipmentId { get; set; }
        public int Quantity { get; set; }
        public Guid? UserAccountId { get; set; }
        public DateTime? ReturnedDealine { get; set; }
    }
}
