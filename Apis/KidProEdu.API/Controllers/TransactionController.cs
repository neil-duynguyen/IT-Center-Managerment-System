using KidProEdu.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransaction()
        { 
            var result = await _transactionService.GetAllTransaction();
            return Ok(result);
        }

        [HttpGet("GetTransactionDetailByTransactionId/{id}")]
        public async Task<IActionResult> GetTransactionDetailByTransactionId(Guid id)
        {
            var result = await _transactionService.GetTransactionDetailByTransactionId(id);
            return Ok(result);
        }


    }
}
