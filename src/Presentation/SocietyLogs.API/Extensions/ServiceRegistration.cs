using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Application.Features.Companies.Commands.Create;
using SocietyLogs.Persistence.Contexts;
using SocietyLogs.Persistence.Interceptors;
using SocietyLogs.Persistence.Repositories;
using System;

namespace SocietyLogs.API.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Interceptor'ı kaydet
            services.AddScoped<AuditInterceptor>();

            // 2. Veritabanını kaydet (MSSQL)
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // 3. Repository ve UnitOfWork'ü bağla
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            // 1. MediatR'ı Application katmanındaki tüm handler'ları bulacak şekilde kaydet
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCompanyHandler).Assembly));

            // 2. FluentValidation'ı kaydet
            services.AddValidatorsFromAssembly(typeof(CreateCompanyValidator).Assembly);
        }
    }
}
