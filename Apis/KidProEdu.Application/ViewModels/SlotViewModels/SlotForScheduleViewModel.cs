using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.SlotViewModels
{
    public class SlotForScheduleViewModel
    {
        public Guid Id { get; set; }
        public string SlotName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SlotType { get; set; }

    }
}
