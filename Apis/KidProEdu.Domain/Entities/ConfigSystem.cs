using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class ConfigSystem : BaseEntity
    {
        public string WebLogo { get; set; }
        public string TextLogo { get; set; }
        public string WebsiteName { get; set; }
        public string Favicon { get; set; }
        public string DefaultAvatar { get; set; }
        public string DefaultPassword { get; set; }

    }
}
