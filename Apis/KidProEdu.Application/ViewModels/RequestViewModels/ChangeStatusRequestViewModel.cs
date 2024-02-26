using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RequestViewModels
{
    public class ChangeStatusRequestViewModel
    {
        public Guid[] ids { get; set; }
        public string status { get; set; }
    }
}
