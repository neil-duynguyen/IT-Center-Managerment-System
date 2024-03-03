using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("Documents")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Documents()
        {
            return Ok(await _documentService.GetDocuments());
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostDocument(CreateDocumentViewModel createDocumentViewModel)
        {
            try
            {
                var result = await _documentService.CreateDocument(createDocumentViewModel);
                if (result)
                {
                    return Ok("Tài liệu đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Tài liệu đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Doument(Guid id)
        {
            var result = await _documentService.GetDocumentById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("DocumentByClass/{classId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DocumentByClassId(Guid classId)
        {
            var result = await _documentService.GetDocumentsByClassId(classId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("DocumentByLesson/{lessonId}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DocumentByLessonId(Guid lessonId)
        {
            var result = await _documentService.GetDocumentsByLessonId(lessonId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutDocument(UpdateDocumentViewModel updateDocumentViewModel)
        {
            try
            {
                var result = await _documentService.UpdateDocument(updateDocumentViewModel);
                if (result)
                {
                    return Ok("Tài liệu đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Tài liệu đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            try
            {
                var result = await _documentService.DeleteDocument(id);
                if (result)
                {
                    return Ok("Tài liệu đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Tài liệu đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
