using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevice.Abstactions;
using Shared.AppointmentDtos;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await service.GetAllAppointmentsAsync();
            return Ok(appointments);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await service.GetAppointmentByIdAsync(id);
            if (appointment is null) return NotFound(new { Error = "Appointment not found" });
            return Ok(appointment);
        }
        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = await service.GetAppointmentsByDoctorIdAsync(doctorId);
            if(!appointments.Any())
                return NotFound(new { Error = "No appointments found for the specified doctor" });
            return Ok(appointments);
        }
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = await service.GetAppointmentsByPatientIdAsync(patientId);
            if (!appointments.Any())
                return NotFound(new { Error = "No appointments found for the specified patient" });
            return Ok(appointments);
        }
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                var result = await service.CreateAppointmentAsync(dto);
                if (result <= 0)
                    return BadRequest(new { Error = "Failed to create appointment" });

                return Ok(new { Message = "Appointment created successfully" });
        }
        [HttpPut]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateAppointment(int id,[FromBody] UpdateAppoinment dto)
        {
            if (id!=dto.Id)
                return BadRequest(new { Error = "ID in URL does not match ID in body" });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await service.UpdateAppoinmentAsync(dto);
            if (result <= 0)
                return NotFound(new { Error = "Appointment not found or failed to update" });
            return NoContent();
        }
        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var result = await service.CancleAppointmentAsync(id);
            if (result <= 0)
                return NotFound(new { Error = "Appointment not found or failed to cancel" });
            return Ok(new { Message = "Appointment cancelled successfully" });
        }
        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            var result = await service.ComletedAppointmentAsync(id);
            if (result <= 0)
                return NotFound(new { Error = "Appointment not found or failed to complete" });
            return Ok(new { Message = "Appointment completed successfully" });
        }
        [HttpGet("upcoming")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var appointments = await service.GetUpcomingAppointmentsAsync();
            if (!appointments.Any())
                return NotFound(new { Error = "No upcoming appointments found" });
            return Ok(appointments);
        }
        [HttpGet("doctor/{doctorid}/upcoming")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetUpcomingAppointmentsForDoctor(int doctorid)
        {
            var appointments = await service.GetUpcomingAppointmentsForDoctorAsync(doctorid);
            if (!appointments.Any())
                return NotFound(new { Error = "No upcoming appointments found for the specified doctor" });
            return Ok(appointments);
        }

    }
}
