using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{

    public class Course : BaseEntity
    {
        public string CourseCode { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DurationTotal { get; set; }
        public string Syllabus { get; set; }
        public int Discount { get; set; }
        public string Level { get; set; }
        public string EntryPoint { get; set; }
        public IList<Prerequisite> Prerequisite { get; set; }
        public IList<Rating> Rating { get; set; }
        public IList<TrainingProgramCourse> TrainingProgramCourse { get; set; }
        public IList<SemesterCourse> SemesterCourse { get; set; }
        public IList<Class> Class { get; set; }
        public IList<Lesson> Lesson { get; set; }
    }
}
