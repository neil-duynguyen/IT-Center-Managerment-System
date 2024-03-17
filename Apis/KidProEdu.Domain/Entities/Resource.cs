using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Resource : BaseEntity
    {
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public virtual Course Course { get; set; }
    }
}
