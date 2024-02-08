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
        public IList<Equipment> Equipments { get; set; }
    }
}
