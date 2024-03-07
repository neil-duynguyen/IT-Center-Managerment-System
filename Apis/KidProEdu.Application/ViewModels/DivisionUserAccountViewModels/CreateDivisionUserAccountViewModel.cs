using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.DivisionUserAccountViewModels
{
    public class CreateDivisionUserAccountViewModel
    {
        public Guid DivisionId { get; set; }
        public Guid UserAccountId { get; set; }
    }
}
