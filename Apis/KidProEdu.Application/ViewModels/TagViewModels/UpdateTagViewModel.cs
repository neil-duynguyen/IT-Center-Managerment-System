using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.TagViewModels
{

    public class UpdateTagViewModel
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
    }
}
