using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Persistence.Contexts;
using SocietyLogs.Persistence.Interceptors;
using SocietyLogs.Persistence.Repositories;
using SocietyLogs.Persistence.Services;

namespace SocietyLogs.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Interceptor
            services.AddScoped<AuditInterceptor>();

            // 2. MSSQL Bağlantısı
            // (Interceptor'ı DbContext'e bağlamayı unutma!)
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var interceptor = sp.GetService<AuditInterceptor>();

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                       .AddInterceptors(interceptor!); // Interceptor'ı buraya gömdük
            });

            // 3. Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IUserService, UserService>();

            // Özel Repository varsa buraya eklenir
            // services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
    }
}