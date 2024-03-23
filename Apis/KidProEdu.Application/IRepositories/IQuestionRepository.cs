using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<List<Question>> GetQuestionByTitle(string title);

        Task<List<Question>> GetQuestionByLesson(Guid id);
        Task<List<Question>> GetQuestionByType(QuestionType type);

    }
}
