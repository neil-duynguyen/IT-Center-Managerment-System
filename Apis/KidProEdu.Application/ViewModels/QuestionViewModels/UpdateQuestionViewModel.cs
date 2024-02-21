using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.QuestionViewModels
{
    public class UpdateQuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid LessionId { get; set; }
        public string Title { get; set; }
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? RightAnswer { get; set; }
        public int? Level { get; set; }
    }
}
