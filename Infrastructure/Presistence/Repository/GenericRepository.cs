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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ClinicDbContext _context;

        public GenericRepository(ClinicDbContext context)
        {
            _context = context;
        }
        public async Task< IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Add(T entity)
        {
             await _context.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _context.Update(entity);   
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            _context.Remove(entity);
            return true;
        }


        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
