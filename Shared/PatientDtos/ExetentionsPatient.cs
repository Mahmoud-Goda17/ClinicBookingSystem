using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Shared.PatientDtos
{
   public static class ExetentionsPatient
    {
        public static PatientDto ToPaientDto(this Patient paient)
        {
            return new PatientDto
            {
                Name = paient.Name,
                Phone = paient.Phone,
            };
        }
        public static Patient ToPatientForCreate(this CreatePaientDto paientDto)
        {
            return new Patient
            {
                Name = paientDto.Name,
                Phone = paientDto.Phone,
                Email = paientDto.Email,
            };
        }
        public static void ToPatientForUpdate(this UpdatePaientDto paientDto, Patient paient)
        {
            paient.Name = paientDto.Name;
            paient.Phone = paientDto.Phone;
            paient.Email = paientDto.Email;
        }

    }
}
