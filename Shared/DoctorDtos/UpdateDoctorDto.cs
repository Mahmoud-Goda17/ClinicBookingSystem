using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DoctorDtos
{
    public class UpdateDoctorDto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Specialty { get; set; }
        [Required, MaxLength(11)]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(200)]
        public string Address { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Range(0, double.MaxValue)]
        public decimal ConsultationFee { get; set; }
    }

}
