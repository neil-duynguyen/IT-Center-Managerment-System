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

    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        private readonly AppDbContext _dbContext;
        public DocumentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Document>> GetDocumentsByClassId(Guid classId)
        {
            return await _dbContext.Document.Where(x => !x.IsDeleted && x.ClassId == classId).ToListAsync();
        }

        public async Task<List<Document>> GetDocumentsByLessonId(Guid lessonId)
        {
            return await _dbContext.Document.Where(x => !x.IsDeleted && x.LessonId == lessonId).ToListAsync();
        }
    }
}
