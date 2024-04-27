using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ContractViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _ContractService;
        public ContractController(IContractService ContractService)
        {
            _ContractService = ContractService;
        }

        [HttpGet("Contracts")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Locations()
        {
            return Ok(await _ContractService.GetContracts());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Location(Guid id)
        {
            var result = await _ContractService.GetContractById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostLocation(CreateContractViewModel createContractViewModel, Guid userId)
        {
            try
            {
                var result = await _ContractService.CreateContract(createContractViewModel, userId);
                if (result)
                {
                    return Ok("Hợp đồng đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Hợp đồng đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutLocation(UpdateContractViewModel updateContractViewModel)
        {
            try
            {
                var result = await _ContractService.UpdateContract(updateContractViewModel);
                if (result)
                {
                    return Ok("Hợp đồng đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Hợp đồng đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            try
            {
                var result = await _ContractService.DeleteContract(id);
                if (result)
                {
                    return Ok("Hợp đồng đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Hợp đồng đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetContractByTeacher/{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> GetContracByTeacherId(Guid id)
        {
            var result = await _ContractService.GetContractByTeacherId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
