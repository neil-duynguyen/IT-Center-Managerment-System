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
            var result = await _transactionService.TransactionsSummarise();
            return Ok(result);
        }

        [HttpGet("TransactionsSummariseByMonthInYear")]
        public async Task<IActionResult> TransactionsSummariseByMonthInYear(DateTime monthInYear)
        {
            var result = await _transactionService.TransactionsSummariseByMonthInYear(monthInYear);
            return Ok(result);
        }

        [HttpGet("TransactionsSummariseByCourses")]
        public async Task<IActionResult> TransactionsSummariseByCourses(DateTime monthInYear)
        {
            var result = await _transactionService.TransactionByCoursesInYear(monthInYear);
            return Ok(result);
        }

        [HttpGet("CourseSummariseDetails")]
        public async Task<IActionResult> CourseSummariseDetails(DateTime dateTime)
        {
            var result = await _courseService.CourseSummariseDetail(dateTime);
            return Ok(result);
        }

        [HttpGet("ChildrenSummariseByYear")]
        public async Task<IActionResult> ChildrenSummariseByYear(DateTime dateTime)
        {
            var result = await _childrenService.GetChildrenSummariseViewModel(dateTime);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDashBoardSummarise(DateTime startTime, DateTime endTime)
        {
            var result = await _transactionService.GetDashBoards(startTime, endTime);
            return Ok(result);
        }
    }
}
