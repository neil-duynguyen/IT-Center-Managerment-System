using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class ChangeTeacherForClassViewModel
    {
        public Guid TeacherId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
