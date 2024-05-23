using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
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
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public TypeOfPractice? TypeOfPractice { get; set; }
        public int? GroupSize { get; set; }
        public IList<Guid>? EquipmentId { get; set; }
    }
}
