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

        public IList<UserAccount> UserAccount { get; set; }
    }
}
