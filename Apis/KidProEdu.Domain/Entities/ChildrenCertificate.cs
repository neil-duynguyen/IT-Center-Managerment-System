using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class ChildrenCertificate : BaseEntity
    {
        [ForeignKey("ChildrenProfile")]
        public Guid? ChildrenProfileId { get; set; }
        [ForeignKey("Certificate")]
        public Guid? CertificateId { get; set; }
        public virtual ChildrenProfile? ChildrenProfile { get; set; }
        public virtual Certificate? Certificate { get; set; }
    }
}
