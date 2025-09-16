using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevice.Abstactions;
using Shared.DoctorDtos;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController(IDoctorService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task< IActionResult> GetAllDoctors()
        {
            var doctors = await  service.GetAllDoctors();
            return Ok(doctors);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await service.GetDoctorByIdAsync(id);
            if (doctor == null)
                return NotFound();
            return Ok(doctor);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDoctor([FromBody]CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await service.AddDoctorAsync(dto);
            if (result <= 0) return BadRequest(new { Error = "Failed to add Patient" });
            return Ok(new { Message = "Doctor added successfully"});
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateDoctor(int id,[FromBody]UpdateDoctorDto dto)
        {
            if (id != dto.Id) return BadRequest("Doctor ID mismatch");
            var result = await service.UpdateDoctorAsync(dto);
            if (result <= 0) return BadRequest(new { Error = "Doctor not found or update failed" });

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await service.DeleteDoctorAsync(id);
            if (result <= 0) return BadRequest(new { Error = "Doctor not found or Delete failed" });
            return NoContent();
        }
        [HttpGet("search-by-name")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { Error = "Name is required" });

            var doctors = await service.SearchDoctorsByNameAsync(name);
            if (!doctors.Any())
                return NotFound(new { Message = "No doctors found with this name" });

            return Ok(doctors);
        }
        [HttpGet("search-by-specialty")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SearchBySpecialty([FromQuery] string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                return BadRequest(new { Error = "Specialty is required" });

            var doctors = await service.SearchBySpecialtyAsync(specialty);
            if (!doctors.Any())
                return NotFound(new { Message = "No doctors found with this specialty" });

            return Ok(doctors);
        }

    }
}
