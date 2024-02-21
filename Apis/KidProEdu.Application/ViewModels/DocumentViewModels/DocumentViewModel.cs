using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.DocumentViewModels
{
    public class DocumentViewModel
    {
        public Guid Id {  get; set; }
        public Guid LessonId { get; set; }
        public string? Url { get; set; }
        public Guid ClassId { get; set; }
    }
}
