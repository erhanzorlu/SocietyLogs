using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SocietyLogs.Application.Common.Interfaces
{
    // Kısıt (Constraint): T, hem class olmalı hem de IEntity sözleşmesine uymalıdır.
    public interface IGenericRepository<T> where T : class, IEntity
    {
        // --- Read (Okuma) İşlemleri (Performans için 'tracking' parametresi var) ---

        // GetSingleAsync'e parametreyi ekliyoruz.
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default);

        // GetWhereAsync zaten parametreye sahipti, dokunmana gerek yok (ama kontrol et).
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default);


        Task<T?> GetByIdAsync(Guid id, bool tracking = true, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default);

        // --- Write (Yazma) İşlemleri (UnitOfWork'te Save çağrılır) ---

        Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);

        bool Update(T entity);

        // --- Delete İşlemleri (Ayrım) ---

        // SOFT DELETE (Mantıksal Silme): IsSoftDeletable kontrolü yapar.
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        // HARD DELETE (Fiziksel Silme): Veritabanından direkt siler.
        void Remove(T entity);

        void RemoveRange(List<T> entities);
    }
}
