using KidProEdu.Application.Interfaces;
using KidProEdu.Application.PaymentService.Payment.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Staff
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrentTime _currentTime;

        public PaymentController(IConfiguration configuration, ICurrentTime currentTime)
        {
            _configuration = configuration;
            _currentTime = currentTime;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string request)
        {
            //CreatePayment _createPayment = new CreatePayment();
            CreatePaymentHandler _paymentHandler = new CreatePaymentHandler(_configuration, _currentTime);

            var result = await _paymentHandler.Handle(request);
            return Ok(result);
        }
    }
}
