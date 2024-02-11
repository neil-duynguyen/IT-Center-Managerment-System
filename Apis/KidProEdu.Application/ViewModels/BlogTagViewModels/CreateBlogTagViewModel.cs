using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.BlogTagViewModels
{
    public class CreateBlogTagViewModel
    {
        public Guid BlogId { get; set; }
        public Guid TagId { get; set; }
    }
}
