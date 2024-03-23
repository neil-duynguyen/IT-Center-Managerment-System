using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.QuestionViewModels
{
    public class CreateExamViewModel
    {
        public Guid? LessonId { get; set; }
        public int? TotalQuestion {  get; set; }
        public QuestionType Type { get; set; }
    }
}
