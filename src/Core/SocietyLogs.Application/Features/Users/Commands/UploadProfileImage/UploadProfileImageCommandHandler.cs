using MediatR;
using Microsoft.AspNetCore.Identity;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Users.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, string>
    {
        private readonly IFileService _fileService;
        private readonly UserManager<AppUser> _userManager;

        public UploadProfileImageCommandHandler(IFileService fileService, UserManager<AppUser> userManager)
        {
            _fileService = fileService;
            _userManager = userManager;
        }

        public async Task<string> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            // 1. Resmi FileService ile diske kaydet
            // "profiles" klasörüne kaydedecek.
            string imagePath = await _fileService.UploadAsync(request.ProfileImage, "profiles");

            if (string.IsNullOrEmpty(imagePath))
            {
                throw new Exception("Dosya yüklenemedi!");
            }

            // 2. Kullanıcıyı bul ve veritabanını güncelle
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user != null)
            {
                // Eğer eski resmi varsa silebiliriz (İsteğe bağlı, şimdilik dursun)
                // _fileService.Delete(user.ProfileImageUrl); 

                user.ProfileUrl = imagePath;
                await _userManager.UpdateAsync(user);
            }

            return imagePath; // Yeni resmin yolunu dön
        }
    }
}
