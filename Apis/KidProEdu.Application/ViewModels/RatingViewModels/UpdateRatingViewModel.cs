using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RatingViewModels
{
    public class UpdateRatingViewModel
    {
        public Guid Id { get; set; }
        public string? Comment { get; set; }
        public string? StarNumber { get; set; }
    }
}
