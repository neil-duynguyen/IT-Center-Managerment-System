using DocumentFormat.OpenXml.EMMA;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.PaymentService.Dtos;
using KidProEdu.Application.PaymentService.Momo.Request;
using KidProEdu.Application.PaymentService.Payment.Commands;
using KidProEdu.Application.PaymentService.VnPay.Response;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Web;

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
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> CreateOrder(CreateOrderDetailViewModel orderDetailViewModel)
        {
            try
            {
                var result = await _orderService.CreateOrder(orderDetailViewModel);
                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Lỗi tạo đơn hàng");
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

        [HttpPost("CreatePayment")]
        [Authorize(Roles = ("Staff"))]
        public async Task<IActionResult> CreatePayment(Guid orderId)
        {
            try
            {
                var result = await _orderService.CreatePaymentHandler(orderId);          
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("momo-return")]
        public async Task<IActionResult> MomoReturn([FromQuery] MomoOneTimePaymentResultRequest response)
        {
            var result = await _orderService.ProcessMomoPaymentReturnHandler(response);

            var redirectUrlWithMessage = $"{result.RedirectUrl}?message={HttpUtility.UrlEncode(result.Message)}&orderNumber={HttpUtility.UrlEncode(result.OrderNumber)}&amount={HttpUtility.UrlEncode(result.Amount.ToString())}";
            // Trả về kết quả Redirect sang trang khác và cùng với thông báo giao dịch
            return Redirect(redirectUrlWithMessage);
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnPayReturn([FromQuery] VnpayPayResponse response)
        {
            var result = await _orderService.ProcessVnPaymentReturnHandler(response);

            var redirectUrlWithMessage = $"{result.RedirectUrl}?message={HttpUtility.UrlEncode(result.Message)}&orderNumber={HttpUtility.UrlEncode(result.OrderNumber)}&amount={HttpUtility.UrlEncode(result.Amount.ToString())}";
            // Trả về kết quả Redirect sang trang khác và cùng với thông báo giao dịch
            return Redirect(redirectUrlWithMessage);
        }

    }
}
