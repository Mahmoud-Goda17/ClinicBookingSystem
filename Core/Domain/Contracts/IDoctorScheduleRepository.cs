using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public interface IDoctorScheduleRepository : IGenericRepository<DoctorSchedule>
    {
        Task<IEnumerable<DoctorSchedule>> GetScheduleByDoctorIdAsync(int doctorId);
        Task<IEnumerable<DoctorSchedule>> GetAvailableSchedulesAsync(int doctorId, DayOfWeek dayOfWeek);
        Task<bool> IsDoctorAvailableAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);
        Task SoftDeleteAsync(int id);

    }
}
