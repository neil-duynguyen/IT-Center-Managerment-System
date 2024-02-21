using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Locations;
using KidProEdu.Application.Validations.Tags;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateLocation(CreateLocationViewModel createLocationViewModel)
        {
            var validator = new CreateLocationViewModelValidator();
            var validationResult = validator.Validate(createLocationViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var location = await _unitOfWork.LocationRepository.GetLocationByName(createLocationViewModel.Name);
            if (!location.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<Location>(createLocationViewModel);
            await _unitOfWork.LocationRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo vị trí thất bại");
        }


        public async Task<bool> DeleteLocation(Guid locationId)
        {
            var result = await _unitOfWork.LocationRepository.GetByIdAsync(locationId);

            if (result == null)
                throw new Exception("Không tìm thấy vị trí này");
            else
            {
                _unitOfWork.LocationRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa vị trí thất bại");
            }
        }

        public async Task<Location> GetLocationById(Guid locationId)
        {
            var location = await _unitOfWork.LocationRepository.GetByIdAsync(locationId);
            return location;
        }

        public async Task<List<Location>> GetLocations()
        {
            var locations = _unitOfWork.LocationRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return locations;
        }

        public async Task<bool> UpdateLocation(UpdateLocationViewModel updateLocationViewModel)
        {
            var validator = new UpdateLocationViewModelValidator();
            var validationResult = validator.Validate(updateLocationViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var location = await _unitOfWork.LocationRepository.GetByIdAsync(updateLocationViewModel.Id);
            if (location == null)
            {
                throw new Exception("Không tìm thấy vị trí");
            }


            var existingLocation = await _unitOfWork.LocationRepository.GetLocationByName(updateLocationViewModel.Name);
            if (!existingLocation.IsNullOrEmpty())
            {
                if (existingLocation.FirstOrDefault().Id != updateLocationViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }

            location.Name = updateLocationViewModel.Name;
            location.Address = updateLocationViewModel.Address;

            _unitOfWork.LocationRepository.Update(location);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật vị trí thất bại");
        }
        
    }
}
