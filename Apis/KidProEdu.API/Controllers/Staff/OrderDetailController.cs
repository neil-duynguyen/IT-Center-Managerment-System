using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
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

        [HttpGet]
        public async Task<IActionResult> GetOrderDetailByOrderId(Guid orderId)
        { 
            var result = await _orderDetailService.GetOrderDetailByOrderId(orderId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderDetail(List<UpdateOrderDetailViewModel> updateOrderDetailView)
        {
            try
            {
                return Ok(await _orderDetailService.UpdateOrderDetail(updateOrderDetailView));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
