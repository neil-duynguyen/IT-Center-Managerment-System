using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Division : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IList<DivisionUserAccount> DivisionUserAccounts { get; set; }
    }
}
