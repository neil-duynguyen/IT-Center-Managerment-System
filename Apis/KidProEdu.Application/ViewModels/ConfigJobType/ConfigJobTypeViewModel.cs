using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ConfigJobType
{
    public class ConfigJobTypeViewModel
    {
        public Guid Id { get; set; }
        public string JobType { get; set; }
        public int Slotperweek { get; set; }
    }
}
