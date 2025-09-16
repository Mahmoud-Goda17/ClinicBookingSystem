using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Shared.DoctorScheduleDtos
{
    public static class ExtintionDoctorSchedule
    {
        public static DoctorScheduleDto ToDoctorScheduleDto(this DoctorSchedule doctorSchedule)
        {
            return new DoctorScheduleDto
            {
                DayOfWeek = doctorSchedule.DayOfWeek,
                StartTime = doctorSchedule.StartTime,
                EndTime = doctorSchedule.EndTime,
            };
        }
        public static DoctorSchedule ToCreateDoctorSchedule(this CreateDoctorScheduleDto dto)
        {
            return new DoctorSchedule
            {
                DoctorId = dto.DoctorId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsAvailable = true,
            };
        }
        public static void ToUpdateDoctorSchedule(this UpdateDoctorScheduleDto dto,DoctorSchedule doctorSchedule)
        {
            doctorSchedule.DoctorId = dto.DoctorId;
            doctorSchedule.DayOfWeek = dto.DayOfWeek;
            doctorSchedule.StartTime = dto.StartTime;
            doctorSchedule.EndTime = dto.EndTime;
            doctorSchedule.IsAvailable = dto.IsAvailable;
            doctorSchedule.UpdatedAt = DateTime.UtcNow;
        }

    }
}
