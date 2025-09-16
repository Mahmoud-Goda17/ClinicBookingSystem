using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.AppointmentDtos;

namespace Sevice.Abstactions
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
        Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<int> CreateAppointmentAsync(CreateAppointmentDto dto);
        Task<int> UpdateAppoinmentAsync( UpdateAppoinment dto);
         Task<int> CancleAppointmentAsync(int id);
        Task<int> ComletedAppointmentAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync();
        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsForDoctorAsync(int doctorid);
    }
}
