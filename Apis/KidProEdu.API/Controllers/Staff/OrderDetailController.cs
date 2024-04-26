using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("GetOrderDetailByOrderId/{id}")]
        public async Task<IActionResult> GetOrderDetailByOrderId(Guid id)
        { 
            var result = await _orderDetailService.GetOrderDetailByOrderId(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> UpdateOrderDetail(List<UpdateOrderDetailViewModel> updateOrderDetailView)
        {
            try
            {
                var result = await _orderDetailService.UpdateOrderDetail(updateOrderDetailView);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrderDetailByOrderIdAfterUpdate/{id}")]
        public async Task<IActionResult> GetOrderDetailByOrderIdAfterUpdate(Guid id)
        {
            try
            {
                var result = await _orderDetailService.GetOrderDetailByOrderIdAfterUpdate(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
    }
}
