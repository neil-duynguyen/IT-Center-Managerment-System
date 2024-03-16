using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.SkillCertificateViewModels
{
    public class UpdateSkillCertificateViewModel
    {
        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid SkillId { get; set; }
        public string Url { get; set; }
    }
}
