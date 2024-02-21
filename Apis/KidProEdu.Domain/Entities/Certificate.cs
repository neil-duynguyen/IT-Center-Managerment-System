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
        public Guid CourseId { get; set; }
        public Guid? TrainingProgramId { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public IList<ChildrenCertificate> ChildrenCertificates { get; set; }
        public virtual Course Course { get; set; }
    }
}
