using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{
    public interface ISlotService
    {
        Task<List<Slot>> GetSlots();
    }
}
