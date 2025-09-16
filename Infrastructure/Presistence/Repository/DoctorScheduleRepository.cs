using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;

namespace Presistence.Repository
{
    public class DoctorScheduleRepository : GenericRepository<DoctorSchedule>, IDoctorScheduleRepository
    {
        private readonly ClinicDbContext _context;
        public DoctorScheduleRepository(ClinicDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DoctorSchedule>> GetScheduleByDoctorIdAsync(int doctorId)
        {
          return await _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId && ds.IsAvailable)
                .ToListAsync();
        }


        public async Task<IEnumerable<DoctorSchedule>> GetAvailableSchedulesAsync(int doctorId, DayOfWeek dayOfWeek)
        {
            return await _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId && ds.DayOfWeek == dayOfWeek && ds.IsAvailable)
                .ToListAsync();
        }


        public Task<bool> IsDoctorAvailableAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            return _context.DoctorSchedules
                .AnyAsync(ds => ds.DoctorId == doctorId &&
                                ds.DayOfWeek == dayOfWeek &&
                                ds.StartTime <= startTime &&
                                ds.EndTime >= endTime &&
                                ds.IsAvailable);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var schedule = await _context.DoctorSchedules.FindAsync(id);
            if (schedule != null)
            {
                schedule.IsAvailable = false;
                _context.DoctorSchedules.Update(schedule);
            }


        }
    }
}
