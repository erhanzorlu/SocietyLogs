using Microsoft.EntityFrameworkCore;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Common;
using SocietyLogs.Persistence.Contexts;
using System.Linq.Expressions;


namespace SocietyLogs.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        // Context ve DbSet protected olmalı ki özel repository'ler miras alabilsin.
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #region Read Operations
        public async Task<T?> GetByIdAsync(Guid id, bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!tracking) query = query.AsNoTracking();

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }



        public async Task<List<T>> GetAllAsync(bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!tracking) query = query.AsNoTracking();

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters(); // 🔓 KİLİDİ AÇAN KOD

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();

            // EKSİK OLAN KISIM BURASIYDI:
            if (ignoreQueryFilters)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            // Önce sorguyu oluştur (Henüz DB'ye gitmedi)
            var query = _dbSet.Where(predicate);

            if (!tracking)
                query = query.AsNoTracking();

            // SENİN KODUNDA EKSİK OLAN KISIM BURASIYDI:
            // Parametre vardı ama işlevi yoktu.
            if (ignoreQueryFilters)
                query = query.IgnoreQueryFilters();

            return await query.ToListAsync(cancellationToken);
        }

        #endregion

        #region Write Operations
        public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return true;
        }

        public async Task<bool> AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            return true;
        }

        public bool Update(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }

        // SOFT DELETE İŞLEMİ (Mantıksal Silme)
        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            // Sadece ISoftDeletable yeteneğine sahip olanları soft delete yap.
            if (entity is ISoftDeletable softDeletableEntity)
            {
                softDeletableEntity.Delete(); // Entity'ye 'silin' emrini veriyoruz (Rich Domain Model)
                _context.Entry(entity).State = EntityState.Modified; // Durumunu Modified yap ki SaveChanges çalışsın.
            }
            else
            {
                // Soft Delete yeteneği yoksa (Ara tablo vs. yanlışlıkla çağrıldıysa), hata fırlat.
                throw new InvalidOperationException($"'{typeof(T).Name}' ISoftDeletable'ı uygulamadığı için Soft Delete yapılamaz. Fiziksel silme için 'Remove' metodunu kullanın.");
            }

            await Task.CompletedTask; // Async imzayı korumak için
        }

        // HARD DELETE İŞLEMİ (Fiziksel Silme)
        public void Remove(T entity)
        {
            // Fiziksel siler. Kullanıcı bilerek çağırmalı.
            _dbSet.Remove(entity);
        }

        public void RemoveRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }


        #endregion
    }
}