using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Doctor:BaseEntity
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public decimal ConsultationFee { get; set; }
        public ICollection<Appointment> Appointments { get; set; }=new List<Appointment>();
        public ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();
    }
}
