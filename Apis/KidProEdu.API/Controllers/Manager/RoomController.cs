using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("Rooms")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Locations()
        {
            return Ok(await _roomService.GetRooms());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Location(Guid id)
        {
            var result = await _roomService.GetRoomById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostLocation(CreateRoomViewModel createRoomViewModel)
        {
            try
            {
                var result = await _roomService.CreateRoom(createRoomViewModel);
                if (result)
                {
                    return Ok("Phòng đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Phòng đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutLocation(UpdateRoomViewModel updateRoomViewModel)
        {
            try
            {
                var result = await _roomService.UpdateRoom(updateRoomViewModel);
                if (result)
                {
                    return Ok("Phòng đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Phòng đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            try
            {
                var result = await _roomService.DeleteRoom(id);
                if (result)
                {
                    return Ok("Phòng đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Phòng đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

