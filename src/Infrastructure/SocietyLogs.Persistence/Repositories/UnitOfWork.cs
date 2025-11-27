using Microsoft.EntityFrameworkCore.Storage;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Common;
using SocietyLogs.Persistence.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable _repositories;

        // Transaction nesnesini burada private tutuyoruz, dışarı sızdırmıyoruz.
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class, IEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T);

            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<T>)_repositories[type]!;
            }

            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

            _repositories.Add(type, repositoryInstance);

            return (IGenericRepository<T>)repositoryInstance!;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // --- TRANSACTION IMPLEMENTASYONU ---

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            // Eğer zaten bir transaction varsa yenisini açma
            if (_currentTransaction != null) return;

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Transaction varsa commit et
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                // Hata olursa geri al
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                // Transaction nesnesini temizle
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            // DbContext'i dispose etmeden önce transaction açıksa onu da kapat
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }
    }
}
