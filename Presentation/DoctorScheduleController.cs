using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevice.Abstactions;
using Shared.DoctorScheduleDtos;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]

    public class DoctorScheduleController(IDoctorScheduleService service) : ControllerBase
    {
        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetScheduleByDoctorId(int doctorId)
        {
            var schedules = await service.GetScheduleByDoctorIdAsync(doctorId);
            return Ok(schedules);
        }
        [HttpGet("doctor/{doctorId}/day/{dayOfWeek}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAvailableSchedules(int doctorId, DayOfWeek dayOfWeek)
        {
            var schedules = await service.GetAvailableSchedulesAsync(doctorId, dayOfWeek);
            return Ok(schedules);
        }
        [HttpGet("doctor/{doctorId}/availability")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> IsDoctorAvailable(int doctorId, [FromQuery] DayOfWeek dayOfWeek, [FromQuery] TimeSpan startTime, [FromQuery] TimeSpan endTime)
        {
            var isAvailable = await service.IsDoctorAvailableAsync(doctorId, dayOfWeek, startTime, endTime);
            if (!isAvailable)
                return Ok(new { IsAvailable = false, Message = "Doctor is not available during the specified time" });
            return Ok(new { IsAvailable = isAvailable });
        }
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> AddDoctorSchedule([FromBody] CreateDoctorScheduleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.AddDoctorSchedulesAsync(dto);
            if (result <= 0) return BadRequest(new { Error = "Failed to add Doctor Schedule" });
            return Ok(new { Message = "Doctor Schedule added successfully" });
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateDoctorSchedule(int id, [FromBody] UpdateDoctorScheduleDto dto)
        {
            if (id != dto.Id) return BadRequest("Doctor Schedule ID mismatch");

            var result = await service.UpdateDoctorScheduleAsync(dto);
            if (result <= 0) return BadRequest(new { Error = "Doctor Schedule not found or update failed" });
            return NoContent();


        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> DeleteDoctorSchedule(int id)
        {
            var result = await service.DeleteDoctorSchedulesAsync(id);
            if (result <= 0) return NotFound(new { Error = "Doctor Schedule not found or delete failed" });
            return NoContent();
        }
        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteDoctorSchedule(int id)
        {
            var result = await service.SoftDelete(id);
            if (result <= 0) return NotFound(new { Error = "Doctor Schedule not found or soft delete failed" });
            return NoContent();
        }
    }
}
