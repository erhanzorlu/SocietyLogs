using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // ILogger için
using SocietyLogs.Application.Common.Wrappers; // ServiceResponse burada
using FluentValidation; // ValidationException için şart
using System.Net;
using System.Text.Json;

namespace SocietyLogs.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Varsayılan değerler (500 Hatası)
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var responseModel = new ServiceResponse<string>("Beklenmedik bir hata oluştu.");
            responseModel.Success = false;

            switch (exception)
            {
                // 1. VALIDATION HATASI (400)
                // FluentValidation'dan fırlatılan hataları yakalar.
                case ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Message = "Girdiğiniz verilerde hatalar var.";
                    // Hataları listeye dolduruyoruz
                    responseModel.Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                    break;

                // 2. BULUNAMADI HATASI (404)
                // Veritabanında ID bulunamazsa fırlatılan hata
                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseModel.Message = "Aradığınız kayıt bulunamadı.";
                    break;

                // 3. YETKİSİZ ERİŞİM (401 - Opsiyonel, genelde Identity halleder ama biz de yakalayabiliriz)
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    responseModel.Message = "Bu işlem için yetkiniz yok.";
                    break;

                // 4. KRİTİK HATALAR (500)
                default:
                    // Sadece buraya düşen (beklenmedik) hataları logluyoruz.
                    // Validation hatalarını loglayıp disk doldurmaya gerek yok.
                    _logger.LogError(exception, "Kritik Sistem Hatası!");

                    // Canlı ortamda (Production) gerçek hata mesajını gizlemek güvenlik gereğidir.
                    // Ama Development ortamındaysan hatayı görebilirsin:
                    responseModel.Message = "Sunucu hatası.";
                    responseModel.Errors = new List<string> { exception.Message };
                    break;
            }

            // JSON Olarak Standart Zarfı Gönder
            var result = JsonSerializer.Serialize(responseModel);
            await context.Response.WriteAsync(result);
        }
    }
}