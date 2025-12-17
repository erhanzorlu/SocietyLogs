using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities; // BaseEntity için
using SocietyLogs.Domain.Entities.Identity; // AppUser için

namespace SocietyLogs.Persistence.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            // DÜZELTME 1: BaseEntity yerine IEntity arayüzü üzerinden dönüyoruz.
            // Böylece hem BaseEntity'leri hem AppUser'ı yakalayabiliriz.
            foreach (var entry in dbContext.ChangeTracker.Entries<IEntity>())
            {
                // --- 1. ADIM: SOFT DELETE (HER İKİSİ İÇİN ORTAK) ---
                // Hem AppUser hem BaseEntity "ISoftDeletable" olduğu için bu kod ikisinde de çalışır.
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable softDeletable)
                {
                    entry.State = EntityState.Modified; // Silmeyi iptal et
                    softDeletable.IsDeleted = true;     // Bayrağı kaldır
                    softDeletable.DeletedDate = DateTime.UtcNow;
                }

                // --- 2. ADIM: TARİHÇE (KİMLİK KONTROLÜ İLE) ---

                // SENARYO A: Standart Entityler (Category, Company vs.)
                if (entry.Entity is BaseEntity baseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            baseEntity.CreatedDate = DateTime.UtcNow;
                            baseEntity.IsDeleted = false;
                            break;
                        case EntityState.Modified:
                            baseEntity.UpdatedDate = DateTime.UtcNow;
                            break;
                    }
                }
                // SENARYO B: Identity Kullanıcısı (AppUser)
                // AppUser, BaseEntity olmadığı için onu burada özel olarak yakalıyoruz.
                else if (entry.Entity is AppUser userEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            userEntity.CreatedDate = DateTime.UtcNow;
                            userEntity.IsDeleted = false;
                            break;
                        case EntityState.Modified:
                            userEntity.UpdatedDate = DateTime.UtcNow;
                            break;
                    }
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}