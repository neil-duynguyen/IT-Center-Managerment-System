using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IRatingService
    {
        Task<List<RatingViewModel>> GetRatings();
        Task<bool> CreateRating(CreateRatingViewModel createRatingViewModel);
        Task<bool> UpdateRating(UpdateRatingViewModel updateRatingViewModel);
        Task<RatingViewModel> GetRatingById(Guid id);
        Task<bool> DeleteRating(Guid id);
        Task<List<RatingViewModel>> GetRatingsByCourseId(Guid courseId);
    }
}
