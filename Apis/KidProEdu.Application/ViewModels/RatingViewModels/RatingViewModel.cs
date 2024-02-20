using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RatingViewModels
{
    public class RatingViewModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string? Comment { get; set; }
        public string? StarNumber { get; set; }
        public DateTime? Date { get; set; }
        public Guid UserId { get; set; }
        public string CourseName { get; set; }
    }
}
