using Infrastructures.Repositories;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.IRepositories;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Infrastructures.Repositories
{
    public class SkillCertificateRepository : GenericRepository<SkillCertificate>, ISkillCertificateRepository
    {
        private readonly AppDbContext _dbContext;
        public SkillCertificateRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<SkillCertificate>> GetListSkillCertificatesByUserAccountId(Guid id)
        {
            var skillCertificates = await _dbContext.SkillCertificate.Where(x =>x.UserAccountId == id && !x.IsDeleted).ToListAsync();
            return skillCertificates;
        }

        public async Task<SkillCertificate> GetSkillCertificateByUserAccountIdAndSkillId(Guid userId, Guid skillId)
        {
            var skillCertificate = await _dbContext.SkillCertificate.FirstOrDefaultAsync(x => x.UserAccountId == userId && x.SkillId == skillId && !x.IsDeleted);
            return skillCertificate;
        }
    }
}
