using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Shared.DoctorDtos
{
    public static class ExtentionsDoctors
    {
        public static DoctorDto ToDoctorDto(this Doctor doctor)
        {
            return new DoctorDto
            {
                Name = doctor.Name,
                Specialty = doctor.Specialty,
                PhoneNumber = doctor.PhoneNumber
            };
        }
        public static Doctor ToDoctorForCreate(this CreateDoctorDto createDoctor)
        {
            return new Doctor
            {
                Name = createDoctor.Name,
                Specialty = createDoctor.Specialty,
                PhoneNumber = createDoctor.PhoneNumber,
                Address = createDoctor.Address,
                Email = createDoctor.Email,
                ConsultationFee = createDoctor.ConsultationFee
            };
        }
        public static void ToDoctorForUpdate(this UpdateDoctorDto updateDoctor, Doctor doctor)
        {
            doctor.Name = updateDoctor.Name;
            doctor.Specialty = updateDoctor.Specialty;
            doctor.PhoneNumber = updateDoctor.PhoneNumber;
            doctor.Address = updateDoctor.Address;
            doctor.Email = updateDoctor.Email;
            doctor.ConsultationFee = updateDoctor.ConsultationFee;
            doctor.UpdatedAt = DateTime.UtcNow;
        }

    }
}
