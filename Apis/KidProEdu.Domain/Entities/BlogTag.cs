using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class BlogTag : BaseEntity
    {
        [ForeignKey("Blog")]
        public Guid BlogId { get; set; }

        [ForeignKey("Tag")]
        public Guid TagId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Tag Tag { get; set; }

    }
}
