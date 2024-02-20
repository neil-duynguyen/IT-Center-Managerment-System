using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.RatingViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Children
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("Ratings")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Ratings()
        {
            return Ok(await _ratingService.GetRatings());
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostRating(CreateRatingViewModel createRatingViewModel)
        {
            try
            {
                var result = await _ratingService.CreateRating(createRatingViewModel);
                if (result)
                {
                    return Ok("Đánh giá đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Đánh giá đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Rating(Guid id)
        {
            var rating = await _ratingService.GetRatingById(id);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpGet("RatingByCourse/{courseId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> RatingByCourseId(Guid courseId)
        {
            var rating = await _ratingService.GetRatingsByCourseId(courseId);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutRating(UpdateRatingViewModel updateRatingViewModel)
        {
            try
            {
                var result = await _ratingService.UpdateRating(updateRatingViewModel);
                if (result)
                {
                    return Ok("Đánh giá đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Đánh giá đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            try
            {
                var result = await _ratingService.DeleteRating(id);
                if (result)
                {
                    return Ok("Đánh giá đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Đánh giá đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
