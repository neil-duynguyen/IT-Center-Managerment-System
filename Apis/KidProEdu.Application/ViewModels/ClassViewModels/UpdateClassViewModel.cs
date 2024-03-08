using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ClassViewModels
{
    public class UpdateClassViewModel
    {
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }
        //public Guid SemesterId { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public StatusOfClass StatusOfClass { get; set; }
        public int MaxNumber { get; set; }
        public int ExpectedNumber { get; set; }
    }
}
