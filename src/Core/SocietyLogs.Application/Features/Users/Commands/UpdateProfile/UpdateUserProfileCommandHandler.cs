using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Application.Common.Wrappers;
using SocietyLogs.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ServiceResponse<bool>>
    {
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserProfileCommandHandler(
            IUserService userService,
            IFileService fileService,
            ICurrentUserService currentUserService)
        {
            _userService = userService;
            _fileService = fileService;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcı Kim? (Güvenlik)
            var userId = _currentUserService.UserId;
            if (userId == null)
                return new ServiceResponse<bool>("Oturum bulunamadı.") { Success = false };

            // 2. Command -> DTO Dönüşümü (Otomatik)
            // İsim, Soyisim, Bio gibi alanlar otomatik kopyalanır.
            var userDto = request.Adapt<UpdateUserProfileDto>();

            // 3. Dosya İşlemleri (Manuel Müdahale)
            // Eğer dosya varsa yükle, dönen URL'i DTO'ya yaz.

            if (request.ProfileFile != null)
            {
                userDto.ProfileUrl = await _fileService.UploadAsync(request.ProfileFile, "users/avatars");
            }

            if (request.BannerFile != null)
            {
                userDto.BannerUrl = await _fileService.UploadAsync(request.BannerFile, "users/banners");
            }

            // 4. Servise DTO gönder
            var result = await _userService.UpdateUserProfileAsync(userId.Value, userDto);

            if (!result)
                return new ServiceResponse<bool>("Güncelleme başarısız.") { Success = false };

            return new ServiceResponse<bool>(true, "Profil başarıyla güncellendi.");
        }
    }
}
