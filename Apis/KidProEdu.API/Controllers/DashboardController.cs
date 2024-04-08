using KidProEdu.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly ITransactionService _transactionService;

        public DashboardController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        [HttpGet("TransactionsSummarise")]
        public async Task<IActionResult> GetAllTransactionsSummarise()
        {
            var result = await _transactionService.TransactionsSummarise();
            return Ok(result);
        }

        [HttpGet("TransactionsSummariseByMonthInYear")]
        public async Task<IActionResult> TransactionsSummariseByMonthInYear(DateTime monthInYear)
        {
            var result = await _transactionService.TransactionsSummariseByMonthInYear(monthInYear);
            return Ok(result);
        }
    }
}
