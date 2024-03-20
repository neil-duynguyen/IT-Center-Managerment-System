using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.SkillCertificateViewModels
{
    public class SkillCertificateViewModel
    {
        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid SkillId { get; set; }
        public string Url { get; set; }
    }
}
