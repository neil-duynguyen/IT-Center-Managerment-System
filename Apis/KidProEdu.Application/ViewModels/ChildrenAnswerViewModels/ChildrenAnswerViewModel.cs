using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ChildrenAnswerViewModels
{
    public class ChildrenAnswerViewModel
    {
        public Guid Id { get; set; }
        public Guid ChildrenProfileId { get; set; }
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public string Answer { get; set; }
        public double ScorePerQuestion { get; set; }
    }
}
