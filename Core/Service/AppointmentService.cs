using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Sevice.Abstactions;
using Shared.AppointmentDtos;

namespace Service
{
    public class AppointmentService(IAppointmentRepository repository,IDoctorScheduleRepository doctorSchedule , IDoctorRepository doctorRepository,IPatientRepository patientRepository) : IAppointmentService
    {

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = await repository.GetAllAppointmentsAsync();
            return appointments.Select(a => a.ToAppointment());
        }
        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            var appointment = await repository.GetAppointmentByIdAsync(id);
            if (appointment is null) return null;
            return appointment.ToAppointment();
        }
        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            var doctor = await doctorRepository.GetById(doctorId);
            if (doctor is null) throw new Exception("Doctor not found");

            var appointments = await repository.GetAppointmentsByDoctorIdAsync(doctorId);
            return appointments.Select(a => a.ToAppointment());
        }
        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            var patient = await patientRepository.GetById(patientId);
            if (patient is null) throw new Exception("Patient not found");

            var appointments = await repository.GetAppointmentsByPatientIdAsync(patientId);
            return appointments.Select(a => a.ToAppointment());
        }
        public async Task<int> CreateAppointmentAsync(CreateAppointmentDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
            {
                throw new Exception("End time must be after start time");
            }
            var isDoctorAvailable = doctorSchedule.IsDoctorAvailableAsync(dto.DoctorId, dto.StartTime.DayOfWeek, dto.StartTime.TimeOfDay, dto.EndTime.TimeOfDay);
            if (!isDoctorAvailable.Result)
            {
                throw new Exception("Doctor is not available at the requested time");
            }
            var hasConflict = repository.HasConflictAsync(dto.DoctorId, dto.StartTime, dto.EndTime);
            if (hasConflict.Result)
            {
                throw new Exception("The requested time slot is already booked");
            }
            var appointment = dto.ToCreateAppoinment();
            await repository.Add(appointment);
            return await repository.SaveChanges();
        }

        public async Task<int> UpdateAppoinmentAsync(UpdateAppoinment dto)
        {
            if (dto.EndTime <= dto.StartTime)
            {
                throw new Exception("End time must be after start time");
            }
            var appointment = await repository.GetById(dto.Id);
            if (appointment is null) return 0;

            if(appointment.StartTime!=dto.StartTime||appointment.EndTime!=dto.EndTime)
            {
                var isDoctorAvailable = doctorSchedule.IsDoctorAvailableAsync(appointment.DoctorId, dto.StartTime.DayOfWeek, dto.StartTime.TimeOfDay, dto.EndTime.TimeOfDay);
                if (!isDoctorAvailable.Result)
                {
                    throw new Exception("Doctor is not available at the requested time");
                }
            }
            var hasConflict = repository.HasConflictAsync(appointment.DoctorId, dto.StartTime, dto.EndTime);
            if (hasConflict.Result)
            {
                throw new Exception("The requested time slot is already booked");
            }
            appointment.ToUpdateAppointment(dto);
            repository.Update(appointment);
            return await repository.SaveChanges();

        }

        public async Task<int> CancleAppointmentAsync(int id)
        {
            var appointment = await repository.GetById(id);
            if (appointment is null) return 0;

            if(appointment.Status == AppointmentStatus.Cancelled)
                throw new Exception("Appointment is already cancelled");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;

            repository.Update(appointment);
            return await repository.SaveChanges();
        }

        public async Task<int> ComletedAppointmentAsync(int id)
        {
            var appointment = await repository.GetById(id);
            if (appointment is null) return 0;

            if (appointment.Status != AppointmentStatus.Scheduled)
                throw new Exception("Only scheduled appointments can be marked as completed");

            appointment.Status = AppointmentStatus.Completed;
            appointment.UpdatedAt = DateTime.UtcNow;

            repository.Update(appointment);
            return await repository.SaveChanges();


        }
        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync()
        {
            var appointments = await repository.GetUpcomingAppointmentsAsync();
            return appointments.Select(a => a.ToAppointment());
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsForDoctorAsync(int doctorid)
        {
            var doctor = doctorRepository.GetById(doctorid);
            if (doctor is null) throw new Exception("Doctor not found");

            var appointments = await repository.GetUpcomingAppointmentsForDoctorAsync(doctorid);
            return appointments.Select(a => a.ToAppointment());


        }
    }
}
