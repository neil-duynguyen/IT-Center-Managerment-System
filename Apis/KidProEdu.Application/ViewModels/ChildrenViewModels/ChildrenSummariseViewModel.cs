using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ChildrenViewModels
{
    public class ChildrenSummariseViewModel
    {
        public int TotalChildren { get; set; }
        public List<ChildrenSummariseByMonthViewModel> childrenSummariseByMonthViewModels { get; set; }
    }
}
