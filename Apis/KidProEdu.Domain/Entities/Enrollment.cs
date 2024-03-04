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
        [ForeignKey("ChildrenProfile")]
        public Guid ChildrenProfileId { get; set;}
        public DateTime? RegisterDate { get; set; }
        public double? Commission { get; set; }
        public Guid UserId { get; set; }

        public virtual Class Class { get; set; }
        public virtual ChildrenProfile ChildrenProfile { get; set; }
    }
}
