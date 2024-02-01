using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        [ForeignKey("Class")]
        public Guid ClassId { get; set; }
        [ForeignKey("Children")]
        public Guid ChildrenId { get; set;}
        public DateTime? RegisterDate { get; set; }
        public double? Price { get; set; }
        public Guid UserId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Children Children { get; set; }
    }
}
