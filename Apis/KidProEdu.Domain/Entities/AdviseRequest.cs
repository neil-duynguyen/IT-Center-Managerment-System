using System.ComponentModel.DataAnnotations.Schema;

namespace KidProEdu.Domain.Entities
{

    public class AdviseRequest : BaseEntity
    {
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CourseName { get; set; }
        public bool? IsTested { get; set; }
        public virtual User? User { get; set; }
    }
}
