using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Common.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // Sihirli Repo Metodu
        IGenericRepository<T> Repository<T>() where T : class, IEntity;

        // Değişiklikleri kaydet
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // --- DÜZELTME: Transaction Yönetimi ---
        // IDbContextTransaction (EF Core nesnesi) döndürmek yerine, yönetimi içeri alıyoruz.

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
