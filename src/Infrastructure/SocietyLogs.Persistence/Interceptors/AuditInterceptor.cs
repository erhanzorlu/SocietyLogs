using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

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

            foreach (var entry in dbContext.ChangeTracker.Entries<BaseEntity>())
            {
                // 1. EKLEME (INSERT)
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.IsDeleted = false; // Garanti olsun
                }

                // 2. GÜNCELLEME (UPDATE)
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }

                // 3. SİLME (SOFT DELETE) - Vizyon Hamlesi
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified; // Silme işlemini iptal et, güncellemeye çevir
                    entry.Entity.IsDeleted = true;      // Silindi işaretle
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
