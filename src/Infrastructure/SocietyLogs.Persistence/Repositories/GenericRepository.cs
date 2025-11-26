using Microsoft.EntityFrameworkCore;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Common;
using SocietyLogs.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            // Sadece Context'e ekle, Kaydetme! (Onu UnitOfWork yapacak)
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
