using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RequestUserAccountViewModels
{
    public class CreateRequestUserAccountViewModel
    {
        public Guid[] RecieverIds { get; set; }
        public Guid RequestId { get; set; }
        public StatusOfRequest Status {  get; set; }
    }
}
