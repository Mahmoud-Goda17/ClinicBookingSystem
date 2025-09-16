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
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly ClinicDbContext _context;

        public DoctorRepository(ClinicDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> SearchByNameAsync(string name)
        {
            return await _context.Doctors
                    .Where(d => d.Name.ToLower().Contains(name.ToLower()))
                    .ToListAsync();
        }
        public async Task<IEnumerable<Doctor>> SearchBySpecialtyAsync(string specialty)
        {
            return await _context.Doctors
                    .Where(d => d.Specialty.ToLower().Contains(specialty.ToLower()))
                    .ToListAsync();
        }
    }
}
