using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TeachingClassHistoryViewModels
{
    public class TeachingClassHistoryViewModel
    {
        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TeachingStatus { get; set; }
    }
}
