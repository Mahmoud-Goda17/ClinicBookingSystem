using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared.PatientDtos;

namespace Sevice.Abstactions
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto?> GetPatientByIdAsync(int id);
        Task<int> AddPatientAsync(CreatePaientDto patientdto);
        Task<int> UpdatePatientAsync(UpdatePaientDto patient);
        Task<int> DeletePatientAsync(int id);
        Task<IEnumerable<PatientDto>> SearchByNameAsync(string name);
    }
}
