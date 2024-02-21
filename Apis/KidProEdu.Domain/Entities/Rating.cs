using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Rating : BaseEntity
    {
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        public string? Comment { get; set; }
        public string? StarNumber { get; set; }
        public DateTime? Date { get; set; }
        public Guid UserId { get; set; }

        public virtual Course Course { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
