﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RequestUserAccountViewModels
{
    public class CreateRequestUserAccountViewModel
    {
        public Guid RequestId { get; set; }
        public Guid[] RecieverIds { get; set; }
    }
}