using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Domain.Entities;
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
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Classes()
        {
            return Ok(await _classService.GetClasses());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
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
        /*[Authorize(Roles = ("Admin"))]*/
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
        /*[Authorize(Roles = ("Admin"))]*/
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
        /*[Authorize(Roles = ("Admin"))]*/
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
        /*[Authorize(Roles = ("Admin"))]*/
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
            "Nhập điểm.xlsx");

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
    }
}
