using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace SocietyLogs.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
          where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 1. İsteğin Adını Al (Örn: CreateCategoryCommand)
            var requestName = typeof(TRequest).Name;

            // 2. İsteğin İçeriğini JSON Yap (Logda görmek için)
            // Not: İleride şifre gibi hassas verileri buradan maskelememiz gerekecek.
            var requestContent = JsonSerializer.Serialize(request);

            _logger.LogInformation("🚀 [START] {Name} başladı. İstek: {Request}", requestName, requestContent);

            var timer = new Stopwatch();
            timer.Start();

            try
            {
                // 3. Handler'ı Çalıştır (Esas İş)
                var response = await next();

                timer.Stop();

                // 4. Başarılı Bitiş Logu
                var elapsedMilliseconds = timer.ElapsedMilliseconds;

                // Eğer işlem 500ms'den uzun sürdüyse UYARI (Warning) logu at ki gözümüze çarpsın.
                if (elapsedMilliseconds > 500)
                {
                    _logger.LogWarning("⚠️ [PERFORMANCE] {Name} uzun sürdü ({Elapsed} ms).", requestName, elapsedMilliseconds);
                }
                else
                {
                    _logger.LogInformation("✅ [END] {Name} tamamlandı. Süre: {Elapsed} ms", requestName, elapsedMilliseconds);
                }

                return response;
            }
            catch (Exception)
            {
                // Hata olursa burada yakalamaya gerek yok, zaten Global Exception Middleware yakalayacak.
                // Biz sadece süreyi durduralım.
                timer.Stop();
                throw;
            }
        }
    }
}
