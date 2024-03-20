using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ResourceViewModels
{
    public class CreateResourceViewModel
    {
        public Guid CourseId { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
    }
}
