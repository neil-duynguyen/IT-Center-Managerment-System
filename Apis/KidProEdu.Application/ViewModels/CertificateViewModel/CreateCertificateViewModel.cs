using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.CertificateViewModel
{
    public class CreateCertificateViewModel
    {
        public Guid ChildrenProfileId { get; set; }
        public Guid CourseId { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
    }
}
