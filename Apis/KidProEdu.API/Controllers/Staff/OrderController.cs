using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Orders")]
        [Authorize(Roles = ("Staff"))]
        //api này dùng bên order
        public async Task<IActionResult> GetOrderByStaffId()
        {
            return Ok(await _orderService.GetOrderByStaffId());
        }

        [HttpPost("CreateOrder")]
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

        /*[HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction(Guid orderId)
        {
            try
            {
                *//*var result = await _orderService.CreateTransaction(orderId);
                return Ok(result);*//*
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Lỗi DB");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

    }
}
