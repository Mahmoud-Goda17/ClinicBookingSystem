using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DoctorScheduleDtos;

namespace Shared.DoctorDtos
{
    public class DoctorWithScheduleDto
    {
        
            public int DoctorId { get; set; }
            public string Name { get; set; }
            public string Specialty { get; set; }
            public List<DoctorScheduleDto> Schedules { get; set; } = new();

    }
}
