using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Blog : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public virtual User User { get; set; }
        public IList<BlogTag> BlogTags { get; set; }
    }
}
