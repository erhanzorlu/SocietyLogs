using Microsoft.EntityFrameworkCore;
using SocietyLogs.Domain.Entities;
using SocietyLogs.Persistence.Interceptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SocietyLogs.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly AuditInterceptor _auditInterceptor;

        public AppDbContext(DbContextOptions<AppDbContext> options, AuditInterceptor auditInterceptor)
            : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        // Tablolarımız
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyPosition> CompanyPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurations klasöründeki ayarları otomatik uygula
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Interceptor'ı sisteme tanıt
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }
    }
}
