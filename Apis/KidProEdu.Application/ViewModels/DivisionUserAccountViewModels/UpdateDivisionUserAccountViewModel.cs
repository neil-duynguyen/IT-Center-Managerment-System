using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.DivisionUserAccountViewModels
{


    public class UpdateDivisionUserAccountViewModel
    {
        public Guid Id { get; set; }
        public Guid DivisionId { get; set; }
        public Guid UserAccountId { get; set; }
    }
}
