﻿using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KidProEdu.API.Controllers.Manager
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("Schedules")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Schedules()
        {
            return Ok(await _scheduleService.GetSchedules());
        }

        [HttpGet("{id}")]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> Schedule(Guid id)
        {
            var result = await _scheduleService.GetScheduleById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PostSchedule(CreateScheduleViewModel createScheduleViewModel)
        {
            try
            {
                var result = await _scheduleService.CreateSchedule(createScheduleViewModel);
                if (result)
                {
                    return Ok("Lịch đã được tạo thành công.");
                }
                else
                {
                    return BadRequest("Lịch đã được tạo thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> PutSchedule(UpdateScheduleViewModel updateScheduleViewModel)
        {
            try
            {
                var result = await _scheduleService.UpdateSchedule(updateScheduleViewModel);
                if (result)
                {
                    return Ok("Lịch đã được cập nhật thành công.");
                }
                else
                {
                    return BadRequest("Lịch đã được cập nhật thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        /*[Authorize(Roles = ("Admin"))]*/
        public async Task<IActionResult> DeleteSchedule(Guid id)
        {
            try
            {
                var result = await _scheduleService.DeleteSchedule(id);
                if (result)
                {
                    return Ok("Lịch đã được xóa thành công.");
                }
                else
                {
                    return BadRequest("Lịch đã được xóa thất bại.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
