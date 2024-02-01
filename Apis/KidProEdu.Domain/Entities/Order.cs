using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? ImportDate { get; set; }
        public DateTime? WarrantyDate { get; set; }

        public IList<Equipment> Equipments { get; set; }
    }
}
