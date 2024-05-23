using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.LessonViewModels
{
    public class LessonViewModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public int? LessonNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public string? TypeOfPractice { get; set; }
        public int? GroupSize { get; set; }
        public List<string> CategoryEquipmentsName { get; set; }
    }
}
