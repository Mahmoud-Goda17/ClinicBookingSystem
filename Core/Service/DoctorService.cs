using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Sevice.Abstactions;
using Shared.DoctorDtos;
using Shared.DoctorScheduleDtos;

namespace Service
{
    public class DoctorService(IDoctorRepository repository) : IDoctorService
    {
        public async Task<IEnumerable<DoctorDto>> GetAllDoctors()
        {
            var doctors = await repository.GetAll();
            var result = doctors.Select(d => d.ToDoctorDto());
            return result;
        }

        

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            var doctor = await repository.GetById(id);
            var result = doctor == null ? null : doctor.ToDoctorDto();
            return result;
        }

        public async Task<int> AddDoctorAsync(CreateDoctorDto dto)
        {
            var exists = ( await repository.GetAll())
                                           .Any(d => d.Email == dto.Email || d.PhoneNumber == dto.PhoneNumber);

            if (exists)
                throw new InvalidOperationException("Doctor with same email or phone number already exists.");
            var doctor = dto.ToDoctorForCreate();
            await repository.Add(doctor);
            return await repository.SaveChanges();
        }
        public async Task<int> UpdateDoctorAsync(UpdateDoctorDto doctorDto)
        {
            var Doctor = await repository.GetById(doctorDto.Id);
            if (Doctor is null) return 0;

            var exists = ( await repository.GetAll())
                                         .Any(d =>( d.Email == doctorDto.Email || d.PhoneNumber == doctorDto.PhoneNumber) && d.Id != doctorDto.Id);

            if (exists)
                throw new InvalidOperationException("Doctor with same email or phone number already exists.");

             doctorDto.ToDoctorForUpdate(Doctor);
            repository.Update(Doctor);
            return await repository.SaveChanges();
        }

        public async Task<int> DeleteDoctorAsync(int id)
        {
            var doctor = await repository.GetById(id);
            if (doctor is null) return 0;
            var success = await repository.Delete(id);
            if (!success) return 0 ;
            return await repository.SaveChanges();
        }

        public async Task<IEnumerable<DoctorDto>> SearchDoctorsByNameAsync(string name)
        {
            var doctors = await repository.SearchByNameAsync(name);
            return doctors.Select(d => d.ToDoctorDto());
        }
        public async Task<IEnumerable<DoctorDto>> SearchBySpecialtyAsync(string specialty)
        {
            var doctors = await repository.SearchBySpecialtyAsync(specialty);
            return doctors.Select(d => d.ToDoctorDto());
        }

    }

    }

