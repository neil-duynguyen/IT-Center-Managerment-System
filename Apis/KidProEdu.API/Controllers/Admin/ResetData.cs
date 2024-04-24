using KidProEdu.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidProEdu.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ResetData : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public ResetData(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ResetDataRoleAdmin()
        {
            try
            {
                await _dbContext.ScheduleRoom.ExecuteDeleteAsync();
                await _dbContext.Equipment.ExecuteDeleteAsync();
                await _dbContext.Room.ExecuteDeleteAsync();
                await _dbContext.CategoryEquipment.ExecuteDeleteAsync();
                await _dbContext.LogEquipment.ExecuteDeleteAsync();
                await _dbContext.ChildrenAnswer.ExecuteDeleteAsync();
                await _dbContext.Resource.ExecuteDeleteAsync();
                await _dbContext.Exam.ExecuteDeleteAsync();
                await _dbContext.Question.ExecuteDeleteAsync();
                await _dbContext.Lesson.ExecuteDeleteAsync();
                await _dbContext.Certificate.ExecuteDeleteAsync();
                await _dbContext.Enrollment.ExecuteDeleteAsync();
                await _dbContext.TeachingClassHistory.ExecuteDeleteAsync();
                await _dbContext.Attendance.ExecuteDeleteAsync();
                await _dbContext.Schedule.ExecuteDeleteAsync();
                await _dbContext.Feedback.ExecuteDeleteAsync();
                await _dbContext.Class.ExecuteDeleteAsync();
                await _dbContext.AdviseRequest.ExecuteDeleteAsync();
                await _dbContext.RequestUserAccount.ExecuteDeleteAsync();
                await _dbContext.Request.ExecuteDeleteAsync();
                await _dbContext.Contract.ExecuteDeleteAsync();
                await _dbContext.Transaction.ExecuteDeleteAsync();
                await _dbContext.OrderDetail.ExecuteDeleteAsync();
                await _dbContext.Course.ExecuteDeleteAsync();
                await _dbContext.Order.ExecuteDeleteAsync();
                await _dbContext.ChildrenProfile.ExecuteDeleteAsync();
                await _dbContext.SkillCertificate.ExecuteDeleteAsync();
                await _dbContext.Skill.ExecuteDeleteAsync();
                _dbContext.UserAccount.RemoveRange(_dbContext.UserAccount.Where(x => !x.UserName.Equals("Admin") && !x.UserName.Equals("Manager") && !x.UserName.Equals("Staff")).ToList());
                await _dbContext.SaveChangesAsync();
                //await _dbContext.Location.ExecuteDeleteAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
