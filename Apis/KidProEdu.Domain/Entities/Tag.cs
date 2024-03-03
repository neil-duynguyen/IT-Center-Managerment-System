using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string TagName { get; set; }
        public string? Description { get; set; }
        public TagType TagType { get; set; }
        public IList<Blog> Blogs { get; set; }
        public IList<SkillTag> SkillTag { get; set; }
    }
}
