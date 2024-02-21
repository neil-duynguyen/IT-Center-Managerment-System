using KidProEdu.Application.Repositories;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        Task<List<Lesson>> GetLessonByName(string name);
        Task<List<Lesson>> GetLessonsByCourseId(Guid CourseId);
    }
}
