using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Shared.AppointmentDtos
{
    public class AppointmentDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal FeePaid { get; set; }
        public string DoctorName { get; set; } 
        public string PatientName { get; set; } 
    }
}
