using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared.DoctorDtos;

namespace Sevice.Abstactions
{
    public interface IDoctorService
    {
       Task<IEnumerable<DoctorDto>> GetAllDoctors();
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<int> AddDoctorAsync(CreateDoctorDto doctorDto);
        Task<int> UpdateDoctorAsync(UpdateDoctorDto doctorDto);
        Task<int> DeleteDoctorAsync(int id);
        Task<IEnumerable<DoctorDto>> SearchDoctorsByNameAsync(string name);
        public Task<IEnumerable<DoctorDto>> SearchBySpecialtyAsync(string specialty);


    }
}
