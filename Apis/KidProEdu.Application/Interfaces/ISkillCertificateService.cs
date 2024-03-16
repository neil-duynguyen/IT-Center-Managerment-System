using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface ISkillCertificateService
    {
        Task<List<SkillCertificateViewModel>> GetSkillCertificates();
        Task<bool> CreateSkillCertificate(CreateSkillCertificateViewModel createSkillCertificateViewModel);
        Task<bool> UpdateSkillCertificate(UpdateSkillCertificateViewModel updateSkillCertificateViewModel);
        Task<SkillCertificateViewModel> GetSkillCertificateById(Guid id);
        Task<bool> DeleteSkillCertificate(Guid id);
        Task<List<SkillCertificateViewModel>> GetListSkillCertificatesByUserAccountId(Guid id);
    }
}
