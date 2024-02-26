using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Utils
{
    public static class SplitGuid
    {
        public static List<Guid> SplitGuids(string inputString)
        {
            List<Guid> guids = new List<Guid>();

            string[] parts = inputString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                if (Guid.TryParse(part.Trim(), out Guid guid))
                {
                    guids.Add(guid);
                }
            }

            return guids;
        }
    }
}
