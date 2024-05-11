using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class CategoryEquipment : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public TypeCategoryEquipment TypeCategoryEquipment { get; set; }
        public string? Code { get; set; }
        public IList<Equipment> Equipments { get; set; }
        public IList<LogEquipment> LogEquipments { get; set; }
        public IList<Lesson> Lessons { get; set; }
    }
}
