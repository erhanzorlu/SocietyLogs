using System.Net;
using System.Text.Json;
using FluentValidation;
using Serilog;

namespace SocietyLogs.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Trafik normal akışında devam etsin...
                await _next(context);
            }
            catch (Exception ex)
            {
                // BİR HATA OLDU! 🚨
                // Akışı durdur, hatayı yakala ve özel cevap hazırla.
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Varsayılan: 500 Internal Server Error (Sunucu Hatası)
            var response = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Title = "Sunucu Hatası",
                Detail = "Beklenmedik bir hata oluştu. Lütfen destek ekibiyle iletişime geçin."
            };

            // Hata Tipine Göre Özelleştirme (Switch Case)
            switch (exception)
            {
                // Eğer hata Validation Hatası ise (Eksik veri vs.)
                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Title = "Doğrulama Hatası";
                    response.Detail = "Girdiğiniz verilerde hatalar var.";
                    response.ValidationErrors = validationEx.Errors
                        .Select(e => e.ErrorMessage).ToList();
                    break;

                // Eğer "Kayıt Bulunamadı" hatası ise (İleride yazacağız)
                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Title = "Bulunamadı";
                    response.Detail = "İstediğiniz kayıt sistemde yok.";
                    break;

                // Diğer bilinmeyen hatalar (Veritabanı koptu, Null Reference vs.)
                default:
                    // Sadece bilinmeyen kritik hataları LOGLA.
                    // Validation hatalarını loglamaya gerek yok, kirlilik yapar.
                    Log.Error(exception, "Beklenmedik Hata!");
                    break;
            }

            // JSON'a çevir ve gönder
            context.Response.StatusCode = response.StatusCode;
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }

    // Kullanıcıya dönecek şık JSON formatı
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public List<string>? ValidationErrors { get; set; }
    }
}