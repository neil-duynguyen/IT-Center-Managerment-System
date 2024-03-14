using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.EnrollmentViewModels
{
    public class EnrollmentViewModel
    {
        public Guid Id { get; set; }
        public string ClassCode { get; set; }
        public DateTime RegisterDate { get; set; }
        public double? Commission { get; set; }
        public string ChildrenName { get; set; }
    }
}
