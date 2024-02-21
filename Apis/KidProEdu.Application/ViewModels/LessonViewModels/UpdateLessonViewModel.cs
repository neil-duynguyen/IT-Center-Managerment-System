using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.LessonViewModels
{

    public class UpdateLessonViewModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public int? LessonNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public string? Prerequisites { get; set; }
    }
}
