using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Interfaces
{

    public interface IRoomService
    {
        Task<List<RoomViewModel>> GetRooms();
        Task<bool> CreateRoom(CreateRoomViewModel createRoomViewModel);
        Task<bool> UpdateRoom(UpdateRoomViewModel updateRoomViewModel);
        Task<RoomViewModel> GetRoomById(Guid roomId);
        Task<bool> DeleteRoom(Guid roomId);
    }
}
