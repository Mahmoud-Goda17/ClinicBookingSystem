using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;

namespace Presistence.Repository
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly ClinicDbContext _context;
        public PatientRepository(ClinicDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Patient>> SearchByNameAsync(string name)
        {
            return await _context.Patients
                .Where(p => p.Name.Contains(name))
                .ToListAsync();
        }
    }
}
