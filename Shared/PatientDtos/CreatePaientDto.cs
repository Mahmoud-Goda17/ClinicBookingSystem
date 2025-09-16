using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.PatientDtos
{
    public class CreatePaientDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(11)]
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }

    }
}
