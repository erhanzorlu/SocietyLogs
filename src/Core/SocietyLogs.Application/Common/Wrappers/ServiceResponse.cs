using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Common.Wrappers
{
    // <T> dememizin sebebi: Bu kutu bazen Guid taşır, bazen Kullanıcı Listesi taşır. T = Type (Tip)
    public class ServiceResponse<T>
    {
        public T Data { get; set; }              // Asıl veri (Örn: Oluşan ID)
        public bool Success { get; set; }        // İşlem başarılı mı? (true/false)
        public string Message { get; set; }      // Kullanıcıya gösterilecek mesaj
        public List<string> Errors { get; set; } // Hata varsa listesi

        // Başarılı senaryo için yapıcı metod
        public ServiceResponse(T data, string message = null)
        {
            Data = data;
            Success = true;
            Message = message;
            Errors = null;
        }

        // Başarısız senaryo için yapıcı metod
        public ServiceResponse(string message)
        {
            Success = false;
            Message = message;
        }

        // Hata listesiyle başarısız senaryo
        public ServiceResponse(string message, List<string> errors)
        {
            Success = false;
            Message = message;
            Errors = errors;
        }
    }
}
