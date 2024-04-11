using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface IConfigPointMultiplierService
    {
        Task<List<ConfigPointMultiplier>> GetConfigPointMultipliers();
        Task<bool> UpdateConfigPointMultiplier(Guid id, double multiplier);
    }
}
