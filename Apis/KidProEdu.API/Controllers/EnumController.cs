using KidProEdu.Application.ViewModels;
using KidProEdu.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        [HttpGet("StatusOfRoom")]
        public async Task<IActionResult> StatusOfRoom()
        {
            List<EnumViewModel> enums = ((StatusOfRoom[])Enum.GetValues(typeof(StatusOfRoom))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("StatusOfEquipment")]
        public async Task<IActionResult> StatusOfEquipment()
        {
            List<EnumViewModel> enums = ((StatusOfEquipment[])Enum.GetValues(typeof(StatusOfEquipment))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("CourseType")]
        public async Task<IActionResult> CourseType()
        {
            List<EnumViewModel> enums = ((CourseType[])Enum.GetValues(typeof(CourseType))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }
        
        [HttpGet("StatusAdviseRequest")]
        public async Task<IActionResult> StatusAdviseRequest()
        {
            List<EnumViewModel> enums = ((StatusAdviseRequest[])Enum.GetValues(typeof(StatusAdviseRequest))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }
        
        [HttpGet("StatusOfContract")]
        public async Task<IActionResult> StatusOfContract()
        {
            List<EnumViewModel> enums = ((StatusOfContract[])Enum.GetValues(typeof(StatusOfContract))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }
        
        [HttpGet("JobType")]
        public async Task<IActionResult> JobType()
        {
            List<EnumViewModel> enums = ((JobType[])Enum.GetValues(typeof(JobType))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("TestType")]
        public async Task<IActionResult> TestType()
        {
            List<EnumViewModel> enums = ((TestType[])Enum.GetValues(typeof(TestType))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("PayType")]
        public async Task<IActionResult> PayType()
        {
            List<EnumViewModel> enums = ((PayType[])Enum.GetValues(typeof(PayType))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("QuestionType")]
        public async Task<IActionResult> QuestionType()
        {
            List<EnumViewModel> enums = ((QuestionType[])Enum.GetValues(typeof(QuestionType))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("SlotType")]
        public async Task<IActionResult> SlotType()
        {
            List<EnumViewModel> enums = ((SlotType[])Enum.GetValues(typeof(SlotType))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }
        
        [HttpGet("TeachingStatus")]
        public async Task<IActionResult> TeachingStatus()
        {
            List<EnumViewModel> enums = ((TeachingStatus[])Enum.GetValues(typeof(TeachingStatus))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("StatusOfClass")]
        public async Task<IActionResult> StatusOfClass()
        {
            List<EnumViewModel> enums = ((StatusOfClass[])Enum.GetValues(typeof(StatusOfClass))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("StatusAttendance")]
        public async Task<IActionResult> StatusAttendance()
        {
            List<EnumViewModel> enums = ((StatusAttendance[])Enum.GetValues(typeof(StatusAttendance))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("StatusOfRequest")]
        public async Task<IActionResult> StatusOfRequest()
        {
            List<EnumViewModel> enums = ((StatusOfRequest[])Enum.GetValues(typeof(StatusOfRequest))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("TypeOfPractice")]
        public async Task<IActionResult> TypeOfPractice()
        {
            List<EnumViewModel> enums = ((TypeOfPractice[])Enum.GetValues(typeof(TypeOfPractice))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }

        [HttpGet("TypeCategoryEquipment")]
        public async Task<IActionResult> TypeCategoryEquipment()
        {
            List<EnumViewModel> enums = ((TypeCategoryEquipment[])Enum.GetValues(typeof(TypeCategoryEquipment))).Select(c => new EnumViewModel() { Value = (int)c, Display = c.ToString() }).ToList();
            return Ok(enums);
        }
    }
}
