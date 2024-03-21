using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TeachingClassHistoryViewModels
{

    public class UpdateTeachingClassHistoryViewModel
    {
        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TeachingStatus { get; set; }
    }
}
