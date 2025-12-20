using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SocietyLogs.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Persistence.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public LocalFileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            // 👇 KRİTİK DÜZELTME BURASI 👇
            // Eğer WebRootPath (wwwroot) null gelirse, ContentRootPath (Proje Ana Dizini) + "wwwroot" kullan diyoruz.
            string rootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

            // Artık rootPath'in dolu olduğundan eminiz, hata vermez.
            string uploadPath = Path.Combine(rootPath, "uploads", folderName);

            // Klasör yoksa oluştur (Bu satır hayat kurtarır)
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", folderName, fileName).Replace("\\", "/");
        }

        public void Delete(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            string fullPath = Path.Combine(_env.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
