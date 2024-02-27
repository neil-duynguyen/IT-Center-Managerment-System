﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{
    public class RequestUserAccount : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid RequestId { get; set; }
        [ForeignKey("Request")]
        public Guid RecieverId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual Request Request { get; set; }
    }
}
