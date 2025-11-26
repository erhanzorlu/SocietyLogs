using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Sadece hafızaya ekler (ChangeTracker)
        Task AddAsync(T entity);

        // Veri okuma
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
    }
}
