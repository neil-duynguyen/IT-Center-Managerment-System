using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ConfigJobType
{
    public class UpdateConfigJobTypeViewModel
    {
        public Guid Id { get; set; }
        public JobType JobType { get; set; }
        public int MinSlot { get; set; }
    }
}
