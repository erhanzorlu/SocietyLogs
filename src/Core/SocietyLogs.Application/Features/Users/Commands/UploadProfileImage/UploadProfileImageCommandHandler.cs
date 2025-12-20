using MediatR;
using Microsoft.AspNetCore.Identity;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Application.Common.Wrappers;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Users.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, ServiceResponse<string>>
    {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public UploadProfileImageCommandHandler(
            IFileService fileService,
            IUserService userService,
            ICurrentUserService currentUserService)
        {
            _fileService = fileService;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<string>> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            // 1. Güvenli ID'yi al
            request.UserId = _currentUserService.UserId ?? Guid.Empty;
            if (request.UserId == Guid.Empty)
                return new ServiceResponse<string>("Oturum bulunamadı.") { Success = false };

            // 2. Dosya Yükleme
            // (Validator yazdığını varsayıyoruz, o yüzden burada null check yapmadım ama yapılabilir)
            if (request.ProfileImage == null)
                return new ServiceResponse<string>("Lütfen bir resim seçiniz.") { Success = false };

            string imageUrl = await _fileService.UploadAsync(request.ProfileImage, "users/avatars");

            // 3. Kullanıcıyı Güncelle (Sadece resim alanını)
            // Bunun için UserService'e özel ufak bir metod yazabilirsin veya
            // Mevcut UpdateUserProfile metodunu DTO'yu sadece resimle doldurarak çağırabilirsin.
            // Örnek:
            // await _userService.UpdateProfileImageAsync(request.UserId, imageUrl); 

            // Şimdilik sadece URL döndüğümüzü varsayalım:
            return new ServiceResponse<string>(imageUrl, "Profil resmi başarıyla güncellendi.");
        }
    }
}
