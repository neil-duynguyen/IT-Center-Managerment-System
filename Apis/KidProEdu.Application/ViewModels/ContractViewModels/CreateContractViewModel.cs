using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.ContractViewModels
{
    public class CreateContractViewModel
    {
        //public Guid UserId { get; set; }
        public Guid ConfigJobTypeId { get; set; }
        public string ContractCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Job { get; set; }
        public string File { get; set; }
        public StatusOfContract StatusOfContract { get; set; }
    }
}
