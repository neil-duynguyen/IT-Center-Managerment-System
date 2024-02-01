using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Certificate : BaseEntity
    {
        [ForeignKey("Children")]
        public Guid ChildrenId { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public virtual Children Children { get; set; }
    }
}
