using KidProEdu.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = ("Admin, Staff, Parent"))]
        [HttpGet]
        public async Task<IActionResult> GetAllTransaction()
        {
            try
            {
                var result = await _transactionService.GetAllTransaction();
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTransactionDetailByTransactionId/{id}")]
        public async Task<IActionResult> GetTransactionDetailByTransactionId(Guid id)
        {
            try
            {

                var result = await _transactionService.GetTransactionDetailByTransactionId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}
