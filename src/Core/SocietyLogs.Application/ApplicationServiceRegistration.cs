using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SocietyLogs.Application.Common.Behaviors; // Behavior'ı unutma
using System.Reflection;

namespace SocietyLogs.Application
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // 1. MediatR ve Behavior Kaydı
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                // Pipeline Behavior (Validation için bu şart!)
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            // 2. FluentValidation
            services.AddValidatorsFromAssembly(assembly);
        }
    }
}