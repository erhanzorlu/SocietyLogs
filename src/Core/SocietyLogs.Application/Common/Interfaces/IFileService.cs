using Microsoft.AspNetCore.Http;

namespace SocietyLogs.Application.Common.Interfaces
{
    public interface IFileService
    {
        // Dosyayı kaydeder, geriye "uploads/images/abc.jpg" gibi bir yol döner.
        Task<string> UploadAsync(IFormFile file, string folderName);

        // Dosya silme (Örn: Profil resmi değişince eskisini silmek için)
        void Delete(string filePath);
    }
}
