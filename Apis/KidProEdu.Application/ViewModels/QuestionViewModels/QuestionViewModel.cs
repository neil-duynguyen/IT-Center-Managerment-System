using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.QuestionViewModels
{
    public class QuestionViewModel
    {
        public string Title { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string RightAnswer { get; set; }
        public string Type { get; set; }
        public string Answer { get; set; }
        public double ScorePerQuestion { get; set; }
    }
}
