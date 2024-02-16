using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public IList<UserAccount> UserAccount { get; set; }
    }
}
