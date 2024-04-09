using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ChildrenViewModels
{
    public class ChildrenSummariseByMonthViewModel
    {
        public int totalByMonth { get; set; }
        public List<ChildrenProfileViewModel> childrens { get; set; }
    }
}
