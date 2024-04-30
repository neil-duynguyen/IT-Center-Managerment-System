using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ChildrenAnswerViewModels
{

    public class CreateChildrenAnswerViewModel
    {
        public Guid ChildrenProfileId { get; set; }
        public Guid ExamId { get; set; }
        //public Guid? QuestionId { get; set; }
        public string? Answer { get; set; }
        public double ScorePerQuestion { get; set; }
    }
}
