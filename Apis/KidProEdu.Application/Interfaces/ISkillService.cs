using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.SkillViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface ISkillService
    {
        Task<List<SkillViewModel>> GetSkills();
        Task<bool> CreateSkill(CreateSkillViewModel createSkillViewModel);
        Task<bool> UpdateSkill(UpdateSkillViewModel updateSkillViewModel);
        Task<SkillViewModel> GetSkillById(Guid id);
        Task<bool> DeleteSkill(Guid id);
        Task<SkillViewModel> GetSkillByName(string name);
    }
}
