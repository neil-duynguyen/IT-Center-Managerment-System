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
        private readonly ICourseService _courseService;
        private readonly IChildrenService _childrenService;

        public DashboardController(ITransactionService transactionService,
            ICourseService courseService, IChildrenService childrenService)
        {
            _transactionService = transactionService;
            _courseService = courseService;
            _childrenService = childrenService;
        }


        [HttpGet("TransactionsSummarise")]
        public async Task<IActionResult> GetAllTransactionsSummarise()
        {
            try
            {
                var result = await _transactionService.TransactionsSummarise();
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TransactionsSummariseByMonthInYear")]
        public async Task<IActionResult> TransactionsSummariseByMonthInYear(DateTime monthInYear)
        {
            try
            {
                var result = await _transactionService.TransactionsSummariseByMonthInYear(monthInYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TransactionsSummariseByCourses")]
        public async Task<IActionResult> TransactionsSummariseByCourses(DateTime monthInYear)
        {

            try
            {
                var result = await _transactionService.TransactionByCoursesInYear(monthInYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CourseSummariseDetails")]
        public async Task<IActionResult> CourseSummariseDetails(DateTime dateTime)
        {

            try
            {
                var result = await _courseService.CourseSummariseDetail(dateTime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChildrenSummariseByYear")]
        public async Task<IActionResult> ChildrenSummariseByYear(DateTime dateTime)
        {
            
            try
            {
                var result = await _childrenService.GetChildrenSummariseViewModel(dateTime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDashBoardSummarise(DateTime startTime, DateTime endTime)
        {
           
            try
            {
                var result = await _transactionService.GetDashBoards(startTime, endTime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
