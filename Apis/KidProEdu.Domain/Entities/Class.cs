using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Class : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid? UserId { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        [ForeignKey("Semester")]
        public Guid SemesterId { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusOfClass StatusOfClass { get; set; }
        public int MaxNumber { get; set; }
        public int ExpectedNumber { get; set; }
        public int ActualNumber { get; set; }
        public virtual UserAccount? UserAccount { get; set; }
        public virtual Course Course { get; set; }
        public virtual Semester Semester { get; set; }
        public IList<Enrollment> Enrollments { get; set; }
        public IList<Feedback> Feedbacks { get; set; }
        public IList<Schedule> Schedules { get; set; }

    }
}
