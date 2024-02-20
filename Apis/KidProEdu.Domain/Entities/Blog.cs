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
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public string Title { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}
