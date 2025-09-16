using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DoctorScheduleDtos;

namespace Sevice.Abstactions
{
    public interface IDoctorScheduleService
    {
        Task<IEnumerable<DoctorScheduleDto>> GetScheduleByDoctorIdAsync(int doctorId);
        Task<IEnumerable<DoctorScheduleDto>> GetAvailableSchedulesAsync(int doctorId, DayOfWeek dayOfWeek);
        Task<bool> IsDoctorAvailableAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);
        Task<int> SoftDelete(int id);
        Task<int> AddDoctorSchedulesAsync(CreateDoctorScheduleDto createDoctorScheduleDto);
        Task<int> UpdateDoctorScheduleAsync( UpdateDoctorScheduleDto updateDoctorScheduleDto);
        Task<int> DeleteDoctorSchedulesAsync(int id);

    }
}
