using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        public  Task<IEnumerable<Doctor>> SearchByNameAsync(string name);
        public Task<IEnumerable<Doctor>> SearchBySpecialtyAsync(string specialty);
    }
}
