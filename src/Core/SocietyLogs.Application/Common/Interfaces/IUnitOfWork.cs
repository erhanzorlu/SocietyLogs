using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        // Tüm değişiklikleri (Transaction) tek seferde veritabanına yazar.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
