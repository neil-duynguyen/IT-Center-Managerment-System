using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.QuestionViewModels
{
    public class QuestionByLessonViewModel
    {
        public Guid? LessonId { get; set; }
        public List<Question> Questions { get; set; }   
        public string? Type { get; set; }
    }
}
