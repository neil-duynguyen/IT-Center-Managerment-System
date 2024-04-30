using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet("Classes")]
        /*[Authorize(Roles = "Manager")]*/
        public async Task<IActionResult> Classes()
        {
            return Ok(await _classService.GetClasses());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = "Manager")]*/
        public async Task<IActionResult> Class(Guid id)
        {
            var Class = await _classService.GetClassById(id);
            if (Class == null)
            {
                return NotFound();
            }
            return Ok(Class);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> PostClass(CreateClassViewModel createClassViewModel)
        {
            try
            {
                var result = await _classService.CreateClass(createClassViewModel);
                if (result)
                {
                    return Ok("Lớp đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Lớp đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> PutClass(UpdateClassViewModel updateClassViewModel)
        {
            try
            {
                var result = await _classService.UpdateClass(updateClassViewModel);
                if (result)
                {
                    return Ok("Lớp đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Lớp đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteClass(Guid ClassId)
        {
            try
            {
                var result = await _classService.DeleteClass(ClassId);
                if (result)
                {
                    return Ok("Lớp đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Lớp đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ChangeStatusClass")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ChangeStatusClass(ChangeStatusClassViewModel changeStatusClassViewModel)
        {
            try
            {
                var result = await _classService.ChangeStatusClass(changeStatusClassViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetChildrenByClassId/{classId}")]
        public async Task<IActionResult> GetChildrenByClassId(Guid classId)
        {
            var result = await _classService.GetChildrenByClassId(classId);
            return Ok(result);
        }

        [HttpGet("ExportExcelFile/{classId}")]
        public async Task<IActionResult> ExportExcelFile(Guid classId) => File(await _classService.ExportExcelFileAsync(classId),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "InputScore.xlsx");

        [HttpPost("ImportScoreExcelFile")]
        public async Task<IActionResult> ImportScoreExcelFile(IFormFile formFile)
        {
            try
            {
                var result = await _classService.ImportScoreExcelFileAsync(formFile);
                return Ok("Nhập điểm thành công");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SendAttachEmail")]
        public async Task<IActionResult> SendAttachEmail()
        {
            try
            {
                var result = await _classService.TestSendAttachEmail();
                return Ok(result);

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpPut("ChangeTeacherForClass")]
        public async Task<IActionResult> ChangeTeacherForClass(ChangeTeacherForClassViewModel changeTeacherForClassViewModel)
        {
            try
            {
                var result = await _classService.ChangeTeacherForClass(changeTeacherForClassViewModel);
                return Ok(result);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetListClassTeachingByTeacher/{teacherId}")]
        public async Task<IActionResult> GetListClassTeachingByTeacher(Guid teacherId)
        {
            try
            {
                return Ok(await _classService.GetListClassTeachingByTeacher(teacherId));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetListClassStatusPending")]
        public async Task<IActionResult> GetListClassStatusPending()
        {
            try
            {
                return Ok(await _classService.GetListClassStatusPending());
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
