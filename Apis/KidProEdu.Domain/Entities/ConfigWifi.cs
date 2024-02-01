using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class ConfigWifi : BaseEntity
    {
        public string NameWifi { get; set; }
        public string WifiIpv4 { get; set; }

    }
}
