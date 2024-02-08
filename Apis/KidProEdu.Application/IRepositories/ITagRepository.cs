using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Repositories
{

    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<List<Tag>> GetTagByTagName(string tagName);
    }
}
