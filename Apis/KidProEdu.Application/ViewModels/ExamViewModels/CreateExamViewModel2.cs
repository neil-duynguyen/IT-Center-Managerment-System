using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ExamViewModels
{
    public class CreateExamViewModel2
    {
        public Guid? CourseId { get; set; }
        public string TestName { get; set; }
        public string TestCode { get; set; }
        public DateTime TestDate { get; set; }
        public int TestDuration { get; set; }
        public TestType TestType { get; set; }
    }
}
