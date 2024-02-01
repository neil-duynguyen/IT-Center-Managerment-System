using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }

        public IList<User> Users { get; set; }
        public IList<LocationTrainingProgram> LocationTrainingPrograms { get; set;}
    }
}
