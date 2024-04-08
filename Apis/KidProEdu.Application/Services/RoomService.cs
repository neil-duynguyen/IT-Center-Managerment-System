using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Locations;
using KidProEdu.Application.Validations.Rooms;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
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
            else if (result.Status == Domain.Enums.StatusOfRoom.Used)
            {
                throw new Exception("Không thể xóa phòng, phòng hiện đang được sử dụng");
            }
            else
            {
                _unitOfWork.RoomRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa phòng thất bại");
            }
        }

        public async Task<RoomViewModel> GetRoomById(Guid roomId)
        {
            var result = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            var mapper = _mapper.Map<RoomViewModel>(result);
            return mapper;
        }

        public async Task<List<RoomViewModel>> GetRooms()
        {
            var results = _unitOfWork.RoomRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<RoomViewModel>>(results);
            return mapper;
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

            var room = await _unitOfWork.RoomRepository.GetByIdAsync(updateRoomViewModel.Id);
            if (room == null)
            {
                throw new Exception("Không tìm thấy phòng");
            }
            else if (room.Status != updateRoomViewModel.Status)
            {
                switch (updateRoomViewModel.Status)
                {
                    case Domain.Enums.StatusOfRoom.Empty:
                        if (room.Status != Domain.Enums.StatusOfRoom.NotAllow)
                        {
                            throw new Exception("Chỉ có thể chuyển trạng thái từ NotAllow thành Empty");
                        }
                        break;
                    case Domain.Enums.StatusOfRoom.Used:
                        if (room.Status != Domain.Enums.StatusOfRoom.Empty)
                        {
                            throw new Exception("Chỉ có thể chuyển trạng thái từ Empty thành Used");
                        }
                        break;
                    case Domain.Enums.StatusOfRoom.NotAllow:
                        if (room.Status != Domain.Enums.StatusOfRoom.Empty)
                        {
                            throw new Exception("Chỉ có thể chuyển trạng thái từ Empty sang NotAllow");
                        }
                        break;
                    default:
                        break;
                }
            }

            var existingRoom = await _unitOfWork.RoomRepository.GetRoomByName(updateRoomViewModel.Name);
            if (!existingRoom.IsNullOrEmpty())
            {
                if (existingRoom.FirstOrDefault().Id != updateRoomViewModel.Id)
                {
                    throw new Exception("Tên đã tồn tại");
                }
            }

            room.Name = updateRoomViewModel.Name;
            room.Status = updateRoomViewModel.Status;

            _unitOfWork.RoomRepository.Update(room);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật phòng thất bại");
        }
    }
}
