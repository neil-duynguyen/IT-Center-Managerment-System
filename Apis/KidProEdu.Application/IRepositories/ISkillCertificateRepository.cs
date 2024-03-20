using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface ISkillCertificateRepository : IGenericRepository<SkillCertificate>
    {
        Task<List<SkillCertificate>> GetListSkillCertificatesByUserAccountId(Guid id);
        Task<SkillCertificate> GetSkillCertificateByUserAccountIdAndSkillId(Guid userId, Guid skillId);
    }
}
