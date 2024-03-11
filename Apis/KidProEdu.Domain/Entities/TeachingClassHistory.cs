using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class TeachingClassHistory : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        [ForeignKey("Class")]
        public Guid ClassId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TeachingStatus TeachingStatus { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual Class Class { get; set; }    
    }
}
