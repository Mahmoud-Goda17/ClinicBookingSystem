using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Sevice.Abstactions;
using Shared.PatientDtos;

namespace Service
{
    public class PatientService(IPatientRepository repository) : IPatientService
    {
        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await repository.GetAll();
            var result = patients.Select(P=> P.ToPaientDto());
            return result;
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            var patient = await repository.GetById(id);
            var result = patient == null ? null : patient.ToPaientDto();
            return result;
        }

        public async Task<int> AddPatientAsync(CreatePaientDto patientdto)
        {
            var exists = (await repository.GetAll())
                                           .Any(p => p.Email == patientdto.Email || p.Phone == patientdto.Phone);
            if (exists)
                throw new InvalidOperationException("Patient with same email or phone number already exists.");

            var patient = patientdto.ToPatientForCreate();
            await repository.Add(patient);
            return await repository.SaveChanges();
        }
        public async Task<int> UpdatePatientAsync(UpdatePaientDto patientdto)
        {
            var exists = (await repository.GetAll())
                                         .Any(p => (p.Email == patientdto.Email || p.Phone == patientdto.Phone) && p.Id != patientdto.Id);
            if (exists)
                throw new InvalidOperationException("Patient with same email or phone number already exists.");

            var Patient = await repository.GetById(patientdto.Id);
            if (Patient is null) return 0;

            patientdto.ToPatientForUpdate(Patient);
            repository.Update(Patient);
            return await repository.SaveChanges();
        }

        public async Task<int> DeletePatientAsync(int id)
        {
            var patient = await repository.GetById(id);
            if (patient is null) return 0;
            var success = await repository.Delete(id);
            if (!success) return 0;
            return await repository.SaveChanges();
        }
        public async Task<IEnumerable<PatientDto>> SearchByNameAsync(string name)
        {
            var patients = await repository.SearchByNameAsync(name);
            return patients.Select(p => p.ToPaientDto());
        }


    }
}
