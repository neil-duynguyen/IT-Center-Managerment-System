using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IDocumentService
    {
        Task<List<DocumentViewModel>> GetDocuments();
        Task<bool> CreateDocument(CreateDocumentViewModel createDocumentViewModel);
        Task<bool> UpdateDocument(UpdateDocumentViewModel updateDocumentViewModel);
        Task<DocumentViewModel> GetDocumentById(Guid id);
        Task<bool> DeleteDocument(Guid id);
        Task<List<DocumentViewModel>> GetDocumentsByLessonId(Guid lessonId);
        Task<List<DocumentViewModel>> GetDocumentsByClassId(Guid classId);
    }
}
