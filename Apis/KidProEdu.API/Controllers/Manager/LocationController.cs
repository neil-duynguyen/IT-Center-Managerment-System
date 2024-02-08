using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.TagViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]/")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("Locations")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Locations()
        {
            return Ok(await _locationService.GetLocations());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Location(Guid id)
        {
            var location = await _locationService.GetLocationById(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostLocation(CreateLocationViewModel createLocationViewModel)
        {
            try
            {
                var result = await _locationService.CreateLocation(createLocationViewModel);
                if (result)
                {
                    return Ok("Vị trí đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Vị trí đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutLocation(UpdateLocationViewModel updateLocationViewModel)
        {
            try
            {
                var result = await _locationService.UpdateLocation(updateLocationViewModel);
                if (result)
                {
                    return Ok("Vị trí đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Vị trí đã được cập nhật thất bại.");
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
                var result = await _locationService.DeleteLocation(id);
                if (result)
                {
                    return Ok("Vị trí đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Vị trí đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
