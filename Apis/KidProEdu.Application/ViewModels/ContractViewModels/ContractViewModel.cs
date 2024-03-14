using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ContractViewModels
{
    public class ContractViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ConfigJobTypeId { get; set; }
        public string ContractCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Job { get; set; }
        public string File { get; set; }
        public string StatusOfContract { get; set; }
        /*public virtual UserAccount UserAccount { get; set; }
        public virtual ConfigJobType ConfigJobType { get; set; }*/
    }
}
