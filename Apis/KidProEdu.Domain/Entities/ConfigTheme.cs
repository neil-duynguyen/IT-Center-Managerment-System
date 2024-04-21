using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Entities
{ 
    public class ConfigTheme : BaseEntity
    {
        [ForeignKey("UserAccount")]
        public Guid UserId { get; set; }
        public string ColorCode { get; set; }
        public string BlurCode { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
