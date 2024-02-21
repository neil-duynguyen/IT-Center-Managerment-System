using KidProEdu.Application.ViewModels.DivisionViewModels;
using KidProEdu.Application.ViewModels.RatingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{


    public interface IDivisionService
    {
        Task<List<DivisionViewModel>> GetDivisions();
        Task<bool> CreateDivision(CreateDivisionViewModel createDivisionViewModel);
        Task<bool> UpdateDivision(UpdateDivisionViewModel updateDivisionViewModel);
        Task<DivisionViewModel> GetDivisionById(Guid id);
        Task<bool> DeleteDivision(Guid id);

    }
}
