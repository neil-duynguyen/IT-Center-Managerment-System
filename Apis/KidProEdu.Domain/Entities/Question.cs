using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Question : BaseEntity
    {
        [ForeignKey("Lesson")]
        public Guid? LessionId { get; set; }
        public string? Title { get; set; }
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? RightAnswer { get; set; }
        public int? Level { get; set; }
        public QuestionType? Type { get; set; }

        public virtual Lesson? Lesson { get; set; }
        public IList<ChildrenAnswer> ChildrenAnswer { get; set; }
    }
}
