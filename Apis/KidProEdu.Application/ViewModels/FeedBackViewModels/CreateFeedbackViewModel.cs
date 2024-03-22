using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.FeedBackViewModels
{

    public class CreateFeedBackViewModel
    {
        public Guid UserId { get; set; }
        public Guid RecipientId { get; set; }
        public Guid? ClassId { get; set; }
        public string? Messages { get; set; }
        public string? Stars { get; set; }
    }
}
