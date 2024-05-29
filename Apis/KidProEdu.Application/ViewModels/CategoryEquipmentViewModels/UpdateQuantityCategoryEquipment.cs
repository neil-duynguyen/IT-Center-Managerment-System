using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CategoryEquipmentViewModels
{
    public class UpdateQuantityCategoryEquipment
    {
        public Guid TeacherId { get; set; }
        public Guid ClassId { get; set; }
        public int Progress { get; set; }
    }
}
