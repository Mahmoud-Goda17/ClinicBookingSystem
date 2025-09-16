using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Shared.AppointmentDtos
{
    public static class ExtintionAppointment
    {
        public static AppointmentDto ToAppointment(this Appointment appointment)
        {
            return new AppointmentDto
            {
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                FeePaid = appointment.FeePaid,
                DoctorName = appointment.Doctor.Name,
                PatientName = appointment.Patient.Name
            };
        }
        public static Appointment ToCreateAppoinment(this CreateAppointmentDto appointmentdto)
        {
            return new Appointment
            {
                StartTime = appointmentdto.StartTime,
                EndTime = appointmentdto.EndTime,
                DoctorId = appointmentdto.DoctorId,
                PatientId = appointmentdto.PatientId,
                Status = AppointmentStatus.Scheduled,
                FeePaid = appointmentdto.FeePaid,
                CreatedAt = DateTime.UtcNow
            };
        }
        public static void ToUpdateAppointment( this Appointment appointment, UpdateAppoinment appointmentdto)
        {
            appointment.StartTime = appointmentdto.StartTime;
            appointment.EndTime = appointmentdto.EndTime;
            appointment.Status = appointmentdto.Status;
            appointment.FeePaid = appointmentdto.FeePaid;
            appointment.UpdatedAt = DateTime.UtcNow;
        }
    }
}
