using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface ILocationService
    {
        Task<List<Location>> GetLocations();
        Task<bool> CreateLocation(CreateLocationViewModel createLocationViewModel);
        Task<bool> UpdateLocation(UpdateLocationViewModel updateLocationViewModel);
        Task<Location> GetLocationById(Guid locationId);
        Task<bool> DeleteLocation(Guid locationId);
    }
}
