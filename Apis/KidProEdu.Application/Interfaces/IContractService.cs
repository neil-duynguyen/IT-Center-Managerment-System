using KidProEdu.Application.ViewModels.ContractViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IContractService
    {
        Task<List<ContractViewModel>> GetContracts();
        Task<bool> CreateContract(CreateContractViewModel createContractViewModel, Guid Id);
        Task<bool> UpdateContract(UpdateContractViewModel updateContractViewModel);
        Task<ContractViewModel> GetContractById(Guid contractId);
        Task<bool> DeleteContract(Guid contractId);
        Task<List<ContractViewModel>> GetContractByTeacherId(Guid teacherId);
    }
}
