using KidProEdu.Application.ViewModels.DivisionUserAccountViewModels;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IDivisionUserAccountService
    {
        Task<List<DivisionUserAccountViewModel>> GetDivisionUserAccounts();
        Task<bool> CreateDivisionUserAccount(CreateDivisionUserAccountViewModel createDivisionUserAccountViewModel);
        Task<bool> UpdateDivisionUserAccount(UpdateDivisionUserAccountViewModel updateDivisionUserAccountViewModel);
        Task<DivisionUserAccountViewModel> GetDivisionUserAccountById(Guid id);
        Task<DivisionUserAccountViewModel> GetDivisionUserAccountByUserId(Guid userId);
        Task<bool> DeleteDivisionUserAccount(Guid id);

    }
}
