using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Sevice.Abstactions;
using Shared.DoctorScheduleDtos;

namespace Service
{
    public class DoctorScheduleService(IDoctorScheduleRepository repository) : IDoctorScheduleService
    {
        public async Task<IEnumerable<DoctorScheduleDto>> GetScheduleByDoctorIdAsync(int doctorId)
        {
            var doctorSchedules = await repository.GetScheduleByDoctorIdAsync(doctorId);
            var result = doctorSchedules.Select(ds => ds.ToDoctorScheduleDto());
            return result;

        }
        public async Task<IEnumerable<DoctorScheduleDto>> GetAvailableSchedulesAsync(int doctorId, DayOfWeek dayOfWeek)
        {
            var doctorSchedules = await repository.GetAvailableSchedulesAsync(doctorId, dayOfWeek);
            var result = doctorSchedules.Select(ds => ds.ToDoctorScheduleDto());
            return result;
        }
        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (startTime >= endTime)
                throw new ArgumentException("Start time must be earlier than end time.");

            return await repository.IsDoctorAvailableAsync(doctorId, dayOfWeek, startTime, endTime);
            
        }
        public async Task<int> AddDoctorSchedulesAsync(CreateDoctorScheduleDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
                throw new InvalidOperationException("End time must be greater than start time.");

            var schedules = await repository.GetScheduleByDoctorIdAsync(dto.DoctorId);

            
            var overlap = schedules.Any(ds =>
                ds.DayOfWeek == dto.DayOfWeek &&
                !(dto.EndTime <= ds.StartTime || dto.StartTime >= ds.EndTime));

            if (overlap)
                throw new InvalidOperationException("Doctor already has a schedule that overlaps with the given time.");

            var doctorSchedule = dto.ToCreateDoctorSchedule();
            await repository.Add(doctorSchedule);
            return await repository.SaveChanges();
        }

        public async Task<int> UpdateDoctorScheduleAsync(UpdateDoctorScheduleDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
                throw new InvalidOperationException("End time must be greater than start time.");

            var doctorSchedule = await repository.GetById(dto.Id);
            if (doctorSchedule is null) return 0;

            var schedules = await repository.GetScheduleByDoctorIdAsync(dto.DoctorId);

            var overlap = schedules.Any(ds =>
                ds.DayOfWeek == dto.DayOfWeek &&
                ds.Id != dto.Id &&
                !(dto.EndTime <= ds.StartTime || dto.StartTime >= ds.EndTime));

            if (overlap)
                throw new InvalidOperationException("Doctor already has a schedule that overlaps with the given time.");

            dto.ToUpdateDoctorSchedule(doctorSchedule);
            repository.Update(doctorSchedule);
            return await repository.SaveChanges();
        }
        public async Task<int> DeleteDoctorSchedulesAsync(int id)
        {
            var doctorSchedule = await repository.GetById(id);
            if (doctorSchedule is null) return 0;
             await repository.Delete(id);
            return await repository.SaveChanges();
        }
        public async Task<int> SoftDelete(int id)
        {
            var doctorSchedule = await repository.GetById(id);
            if (doctorSchedule is null) return 0;
            await repository.SoftDeleteAsync(id);
            return await repository.SaveChanges();
        }

    }
}
