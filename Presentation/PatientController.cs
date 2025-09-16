using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevice.Abstactions;
using Shared.PatientDtos;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController(IPatientService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await service.GetAllPatientsAsync();
            return Ok(patients);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await service.GetPatientByIdAsync(id);
            if (patient is null)
                return NotFound();
            return Ok(patient);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPatient([FromBody] CreatePaientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await service.AddPatientAsync(dto);
            if (result <= 0) return BadRequest(new { Error = "Failed to add Patient" });
            return Ok(new { Message = "Patient added successfully" });
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePaientDto patientdto)
        {
            if (id != patientdto.Id)
                return BadRequest("Id mismatch");
            var result = await service.UpdatePatientAsync(patientdto);
            if (result <= 0) return BadRequest(new { Error = "Paient not found or update failed" });
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await service.DeletePatientAsync(id);
            if (result <= 0) return BadRequest(new { Error = "Paient not found or Delete failed" });
            return NoContent();
        }
        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var patients = await service.SearchByNameAsync(name);
            if (!patients.Any())
                return NotFound(new { Error = "No patients found with the given name" });

            return Ok(patients);
        }
    }
    }
