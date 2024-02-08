using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Locations;
using KidProEdu.Application.Validations.Rooms;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }
        public async Task<bool> CreateRoom(CreateRoomViewModel createRoomViewModel)
        {
            var validator = new CreateRoomViewModelValidator();
            var validationResult = validator.Validate(createRoomViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var room = await _unitOfWork.RoomRepository.GetRoomByName(createRoomViewModel.Name);
            if (!room.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<Room>(createRoomViewModel);
            await _unitOfWork.RoomRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo phòng thất bại");
        }

        public async Task<bool> DeleteRoom(Guid roomId)
        {
            var result = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);

            if (result == null)
                throw new Exception("Không tìm thấy phòng này");
            else
            {
                _unitOfWork.RoomRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa phòng thất bại");
            }
        }

        public async Task<Room> GetRoomById(Guid roomId)
        {
            var result = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            return result;
        }

        public async Task<List<Room>> GetRooms()
        {
            var results = _unitOfWork.RoomRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).ToList();
            return results;
        }

        public async Task<bool> UpdateRoom(UpdateRoomViewModel updateRoomViewModel)
        {
            var validator = new UpdateRoomViewModelValidator();
            var validationResult = validator.Validate(updateRoomViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var result = await _unitOfWork.RoomRepository.GetRoomByName(updateRoomViewModel.Name);
            if (!result.IsNullOrEmpty())
            {
                throw new Exception("Tên đã tồn tại");
            }

            var mapper = _mapper.Map<Room>(updateRoomViewModel);
            _unitOfWork.RoomRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật phòng thất bại");
        }
    }
}
