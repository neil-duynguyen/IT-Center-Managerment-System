using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidProEdu.API.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDetailViewModel orderDetailViewModel)
        {
            try
            {
                var result = await _orderService.CreateOrder(orderDetailViewModel);
                return Ok(result);
            }
            catch (DbUpdateException ex)
            { 
                return BadRequest("Lỗi DB");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
