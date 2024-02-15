using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.BlogTagViewModels
{
    public class UpdateBlogTagViewModel
    {
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }
        public Guid TagId { get; set; }
    }
}
