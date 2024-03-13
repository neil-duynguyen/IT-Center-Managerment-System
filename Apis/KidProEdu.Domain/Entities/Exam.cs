using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public int TestDuration { get; set; }
        public TestType TestType { get; set; }
        public IList<ChildrenAnswer> ChildrenAnswer { get; set; }
    }
}
